using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConsoleApp
{
    public class CheckLinkJob 
    {
        private ILogger _Logger;
        private SiteSettings _SiteSettings;
        private OutputSettings _Output;
        private readonly LinksContext _context;
        private readonly LinkChecker _LinkChecker;

        public CheckLinkJob(ILogger<CheckLinkJob> logger, 
            IOptions<SiteSettings> siteSettings, 
            IOptions<OutputSettings> outputOptions, 
            LinksContext context,
            LinkChecker linkChecker)
        {
            _Logger = logger;
            _SiteSettings = siteSettings.Value;
            _Output = outputOptions.Value;
            _context = context;
            _LinkChecker = linkChecker;
        }

        public void Execute()
        {
            _Logger.LogInformation("Executing...");

            var client = new HttpClient();
            var body = client.GetStringAsync(_SiteSettings.Site);

            _Logger.LogTrace(body.Result);

            var links = _LinkChecker.GetLinks(_SiteSettings.Site, body.Result);
            links.ToList().ForEach(Console.WriteLine);

            //var tempFile = Path.GetTempFileName();
            //File.WriteAllLines(tempFile, links);

            Directory.CreateDirectory(_Output.GetReportDirectory());

            var checkedLinks = _LinkChecker.CheckLinks(links);

            //File.WriteAllLines(path, links);

            var filePath = _Output.GetReportFilePath();
            using (var file = File.CreateText(filePath))
            {
                foreach (var checkedLink in checkedLinks.OrderBy(l => l.IsMissing))
                {
                    var status = checkedLink.IsMissing ? "missing" : "OK";
                    file.WriteLine($"{status} - {checkedLink.Link}");
                    _context.Links.Add(checkedLink);
                }
                _context.SaveChanges();
            }
        }
    }
}

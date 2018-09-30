using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        public void Execute(string site, OutputSettings output, string connectionString)
        {
            var logger = Logs.Factory.CreateLogger<CheckLinkJob>();

            logger.LogInformation("Executing...");

            var client = new HttpClient();
            var body = client.GetStringAsync(site);

            logger.LogTrace(body.Result);

            var links = LinkChecker.GetLinks(site, body.Result);
            links.ToList().ForEach(Console.WriteLine);

            //var tempFile = Path.GetTempFileName();
            //File.WriteAllLines(tempFile, links);

            Directory.CreateDirectory(output.GetReportDirectory());

            var checkedLinks = LinkChecker.CheckLinks(links);

            //File.WriteAllLines(path, links);

            var filePath = output.GetReportFilePath();
            using (var file = File.CreateText(filePath))
            using (var linksContext = new LinksContext(Config.DbContextOptions(connectionString)))
            {
                foreach (var checkedLink in checkedLinks.OrderBy(l => l.IsMissing))
                {
                    var status = checkedLink.IsMissing ? "missing" : "OK";
                    file.WriteLine($"{status} - {checkedLink.Link}");
                    linksContext.Links.Add(checkedLink);
                }
                linksContext.SaveChanges();
            }
        }
    }
}

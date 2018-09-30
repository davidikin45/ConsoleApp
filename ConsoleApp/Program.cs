using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SQLite;
using HtmlAgilityPack;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = new Config(args);

            Logs.Init(config.ConfigurationRoot);
            LinksContext.Init(Config.DbContextOptions(config.ConnectionString));

            //UseMemoryStorage
            //Hangfire.GlobalConfiguration.Configuration.UseSQLiteStorage(config.ConnectionString);
            Hangfire.GlobalConfiguration.Configuration.UseMemoryStorage();

            RecurringJob.AddOrUpdate<CheckLinkJob>("check-link", (j => j.Execute(config.Site, config.Output, config.ConnectionString)), Cron.Minutely);
            RecurringJob.Trigger("check-link");

            var webHost = CreateWebHostBuilder(args, config.ConfigurationRoot).Build();
            var webHostRunning = webHost.RunAsync();

            using (var server = new BackgroundJobServer())
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
            //var lines = new List<string>();
            //string line;
            //while (!String.IsNullOrWhiteSpace(line = Console.ReadLine()))
            //{
            //    lines.Add(line);
            //}
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
              WebHost.CreateDefaultBuilder(args)
              .CaptureStartupErrors(true)
              .UseConfiguration(configuration)
              .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args)
        {
            return CreateWebHostBuilder(args, new Config(args).ConfigurationRoot).Build();
        }
    }
}

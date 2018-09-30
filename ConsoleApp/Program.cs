using Hangfire;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp
{
    class Program
    {
        public static void Main(string[] args)
        {
            var config = Config.Build(args);
          
            var host = CreateWebHostBuilder(args, config).Build();

            using (var scope = host.Services.CreateScope())
            {
               var context = scope.ServiceProvider.GetRequiredService<LinksContext>();
                context.Database.Migrate();
            }

            RecurringJob.AddOrUpdate<CheckLinkJob>("check-link", (j => j.Execute()), Cron.Minutely);
            RecurringJob.Trigger("check-link");

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
              WebHost.CreateDefaultBuilder(args)
              //.ConfigureServices(collection => collection.AddSingleton<ILoggerFactory>(services => Logs.Factory))
              .CaptureStartupErrors(true)
              .UseKestrel(options => options.AddServerHeader = false)
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseConfiguration(configuration)
              .UseStartup<Startup>();
    }
}
 
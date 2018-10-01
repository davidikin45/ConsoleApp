using Hangfire;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;

namespace ConsoleApp
{
    class Program
    {
        public static int Main(string[] args)
        {
            var config = Config.Build(args);
            Logs.Init(config);
            var loggerFactory = Logs.Factory;
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                logger.LogInformation("Getting the motors running...");

                var host = CreateWebHostBuilder(args, config, loggerFactory).Build();

                //Db initialization
                using (var scope = host.Services.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<LinksContext>();
                    context.Database.Migrate();
                }

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {

            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration, ILoggerFactory loggerFactory) =>
            //WebHostBuilder - https://github.com/aspnet/Hosting/blob/3483a3250535da6f291326f3f5f1e3f66ca09901/src/Microsoft.AspNetCore.Hosting/WebHostBuilder.cs
            //WebHost.CreateDefaultBuilder(args) - https://github.com/aspnet/MetaPackages/blob/release/2.1/src/Microsoft.AspNetCore/WebHost.cs
            //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.1  

            new WebHostBuilder()
              .CaptureStartupErrors(true)
              .UseKestrel((builderContext, options) =>
               {
                   options.AddServerHeader = false;
                   options.Configure(builderContext.Configuration.GetSection("Kestrel"));
               })
              .UseContentRoot(Directory.GetCurrentDirectory())
              .ConfigureLogging((builderContext, loggingBuilder) => {
                  loggingBuilder.Services.AddSingleton<ILoggerFactory>(services => loggerFactory);
              })
              .UseConfiguration(configuration)
              //IWebHostBuilder configuration is added to the app's configuration, but the converse isn't true—ConfigureAppConfiguration doesn't affect the IWebHostBuilder configuration
              .UseStartup<Startup>();
    }
}
 
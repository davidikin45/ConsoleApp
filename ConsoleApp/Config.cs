using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    public class Config
    {
        public Config(string[] args)
        {
            //In powershell $env:site=https://www.google.com will create environment variable
            //JSON, CLI arguments, Environment Variables, XML, INI
            //Order matters

            //1. InMemory
            //2. appsettings.json file
            //3. appsettings.{ env.EnvironmentName}.json file
            //4. local User Secrets File #Only in local development environment
            //5. Environment Variables
            //6. Command Line Arguments

            var inMemoryDefaults = new Dictionary<string, string>
            {
                {"site","https://www.google.com" },
                {"output:folder", "reports" }, // /output:folder=reports2
                {"output:file", "links.text" }
            };
            var configBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryDefaults)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddEnvironmentVariables()
                .AddCommandLine(args); // /site=https://www.google.com

            ConfigurationRoot = configBuilder.Build();

            Site = ConfigurationRoot["site"];
            Output = ConfigurationRoot.GetSection("output").Get<OutputSettings>();

            ConnectionString = ConfigurationRoot.GetConnectionString("DefaultConnection");
        }

        public string Site { get; set; }
        public OutputSettings Output { get; set; }
        public string ConnectionString { get; set; }
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public static DbContextOptions<LinksContext> DbContextOptions(string connectionString)
        {
            var builder = new DbContextOptionsBuilder<LinksContext>();
            builder.UseSqlite(connectionString);
            return builder.Options;
        }
    }
}

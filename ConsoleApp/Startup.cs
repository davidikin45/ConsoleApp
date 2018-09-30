using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //services.AddHangfire(config => config.UseSQLiteStorage(connectionString));
            services.AddHangfire(config => config.UseMemoryStorage());

            services.AddTransient<CheckLinkJob>();
            services.AddTransient<LinkChecker>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LinksContext>(options => options.UseSqlite(connectionString));

            services.Configure<OutputSettings>(Configuration.GetSection("output"));
            services.Configure<SiteSettings>(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Logs.Init(loggerFactory, Configuration);

            app.UseHangfireServer();
            app.UseHangfireDashboard("");
        }
    }
}

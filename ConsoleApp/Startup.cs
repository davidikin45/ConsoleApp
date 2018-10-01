using Hangfire;
using Hangfire.Common;
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
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public ILogger Logger { get; }
        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            Logger.LogInformation("Configuring Services");

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<LinksContext>(options => options.UseSqlite(connectionString));

            //services.AddHangfire(config => config.UseSQLiteStorage(connectionString));
            services.AddHangfire(config => config.UseMemoryStorage());

            services.AddTransient<CheckLinkJob>();
            services.AddTransient<LinkChecker>();

            services.Configure<OutputSettings>(Configuration.GetSection("output"));
            services.Configure<SiteSettings>(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, IRecurringJobManager recurringJobManager)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard("");

            recurringJobManager.AddOrUpdate("check-link", Job.FromExpression<CheckLinkJob>(m => m.Execute()), Cron.Minutely(), new RecurringJobOptions());
            recurringJobManager.Trigger("check-link");
        }
    }
}

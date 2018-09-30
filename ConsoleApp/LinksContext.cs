using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    //Add-Migration InitialMigration
    //Update-Database
    public class LinksContext : DbContext
    {
        public DbSet<LinkCheckResult> Links { get; set; }

        public LinksContext(DbContextOptions<LinksContext> options)
            :base(options)
        {

        }

        public static void Init(DbContextOptions<LinksContext> options)
        { 
            using (var context = new LinksContext(options))
            {
                context.Database.Migrate();
            }
        }
    }

    public class LinksContextDesignTimeFactory : IDesignTimeDbContextFactory<LinksContext>
    {
        public LinksContext CreateDbContext(string[] args)
        {
            var config = new Config(new string[0]);
            var dbContextOptions = Config.DbContextOptions(config.ConnectionString);
            return new LinksContext(dbContextOptions);
        }
    }
} 
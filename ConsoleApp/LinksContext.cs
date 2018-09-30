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
    }

    public class LinksContextDesignTimeFactory : IDesignTimeDbContextFactory<LinksContext>
    {
        public LinksContext CreateDbContext(string[] args)
        {
            var config = Config.Build(args);
            var connectionString = config.GetConnectionString("DefaultConnection");
            var dbContextOptions = new DbContextOptionsBuilder<LinksContext>().UseSqlite(connectionString).Options;
            return new LinksContext(dbContextOptions);
        }
    }
} 
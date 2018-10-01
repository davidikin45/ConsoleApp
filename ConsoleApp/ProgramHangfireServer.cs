using ConsoleApp.Hangfire;
using Hangfire;
using Hangfire.MemoryStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public class ProgramHangfireServer
    {
        public static void Main2(string[] args)
        {

            //http://docs.hangfire.io/en/latest/extensibility/using-job-filters.html
            GlobalJobFilters.Filters.Add(new LogEverythingAttribute());

            GlobalConfiguration.Configuration.UseMemoryStorage();

            //http://docs.hangfire.io/en/latest/background-processing/configuring-queues.html
            var options = new BackgroundJobServerOptions
            {
                Queues = new string[]{"default"}
            };

            using (var server = new BackgroundJobServer(options))
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}

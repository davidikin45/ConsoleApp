using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class Logs
    {
        //Trace = 0
        //Debug = 1 -- Developement Level
        //Information = 2 -- Default LoggerFactory Level
        //Warning = 3 -- Production Level
        //Error = 4
        //Critical = 5

        public static ILoggerFactory Factory(IConfiguration configuration)
        {
            var factory = new LoggerFactory();
            factory.AddConsole(configuration.GetSection("Logging"));
            factory.AddDebug(LogLevel.Information); //Only used when working on code so don't need to accept configuration
            factory.AddFile("logs/{Date}.json", isJson: true, minimumLevel: LogLevel.Trace);
            factory.AddSeq(configuration.GetSection("Seq"));

            return factory;

            //structured logging, use seq
            //http://localhost:5341
            //https://getseq.net
            //https://docs.getseq.net/docs/using-aspnet-core
        }
    }
} 

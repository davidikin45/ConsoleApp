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
        //Information = 2 -- Default Factory Level
        //Warning = 3 -- Production Level
        //Error = 4
        //Critical = 5

        public static LoggerFactory Factory = new LoggerFactory();
        public static void Init(IConfiguration configuration)
        {
            Factory.AddConsole(configuration.GetSection("Logging"));
            Factory.AddDebug(LogLevel.Information); //Only used when working on code so don't need to accept configuration
            Factory.AddFile("logs/{Date}.json", isJson: true, minimumLevel: LogLevel.Trace);
            Factory.AddSeq(configuration.GetSection("Seq"));

            //structured logging, use seq
            //http://localhost:5341
            //https://getseq.net
            //https://docs.getseq.net/docs/using-aspnet-core


        }
    }
} 

using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp.Hangfire
{
    //http://docs.hangfire.io/en/latest/extensibility/using-job-filters.html
    public class LogEverythingAttribute : JobFilterAttribute,
    IServerFilter
    {


        public void OnPerforming(PerformingContext context)
        {
            //Logger.InfoFormat("Starting to perform job `{0}`", context.BackgroundJob.Id);
        }

        public void OnPerformed(PerformedContext context)
        {
            //Logger.InfoFormat("Job `{0}` has been performed", context.BackgroundJob.Id);
        }
    }
}

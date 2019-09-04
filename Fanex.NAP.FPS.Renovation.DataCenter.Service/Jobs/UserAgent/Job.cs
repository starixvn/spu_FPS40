namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UserAgent
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Newtonsoft.Json;
    using Quartz;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IUserAgent userAgent;       

        public Job()
        {
            userAgent = new Services.Implement.UserAgent();            
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var drawUserAgent = userAgent.GetUserAgent(AppSettings.TransBatch);

                if(drawUserAgent.Any())
                {
                    var userAgentJson = JsonConvert.SerializeObject(drawUserAgent);

                    userAgent.UpdateUserAgent(userAgentJson);
                }           
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            Console.WriteLine($"User Agent is updated at {DateTime.Now}");

            return Task.FromResult(true);
        }
        
    }
}

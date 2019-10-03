namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UserAgent
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IUserAgent userAgent;
        private const int numberOfUserAgent = 10000;

        public Job()
        {
            userAgent = new Services.Implement.UserAgent();            
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                Core.Utilities.WriteLog("UpdateUserAgent", "Start: " + DateTime.Now.ToLongDateString());

                var drawUserAgent = userAgent.GetUserAgent(numberOfUserAgent);

                if (drawUserAgent.Any())
                {
                    var userAgentJson = JsonConvert.SerializeObject(drawUserAgent);

                    userAgent.UpdateUserAgent(userAgentJson);
                }

                Core.Utilities.WriteLog("UpdateUserAgent", "End: " + DateTime.Now.ToLongDateString());
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

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UserAgent
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
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
                var drawUserAgent = userAgent.GetUserAgent(100);

                var xmlUserAgent = Core.Utilities.XMLConvert(drawUserAgent);

                userAgent.UpdateUserAgent(xmlUserAgent);

            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(true);
        }
        
    }
}

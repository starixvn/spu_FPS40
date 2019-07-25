namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IPRangeService ipRangeService;

        private readonly int batchSize = 10000;

        public Job()
        {
            ipRangeService = new Services.Implement.IPRangeService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var ipDBVersionDate = ipRangeService.GetIPDBVersion();
                var ipLatestIPDBLogDate = ipRangeService.GetLatestUpdatedDateIPTableLog();

                if (ipDBVersionDate.CompareTo(ipLatestIPDBLogDate) >= 0)
                {
                    UpdateIpTableV6(ipDBVersionDate, 0, batchSize);

                    ipRangeService.InsertNewTableLog(ipDBVersionDate);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(true);
        }

        private void UpdateIpTableV6(DateTime updatedDate, int fromId, int toId)
        {
            var ipV6Ranges = ipRangeService.GetIpV6Range(fromId, toId);

            if (ipV6Ranges != null && ipV6Ranges.Any())
            {
                var chunkIpV6Range = ipV6Ranges.Chunk(1000);

                foreach(var ipRange in chunkIpV6Range)
                {
                    ipRangeService.UpdateIpRange(ipRange, updatedDate);
                }               

                fromId = (toId + 1);
                toId += batchSize;

                UpdateIpTableV6(updatedDate, fromId, toId);
            }
        }
    }
}

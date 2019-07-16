namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
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
        private readonly IIPRangeService iPRangeService;

        private readonly int batchSize = 10000;

        public Job()
        {
            iPRangeService = new IPRangeService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ipDBVersionDate = iPRangeService.GetIPDBVersion();
            var ipLatestIPDBLogDate = iPRangeService.GetLatestUpdatedDateIPTableLog();

            if (ipDBVersionDate.CompareTo(ipLatestIPDBLogDate) >= 0)
            {
                UpdateIpTableV6(ipDBVersionDate, 0, batchSize);

                iPRangeService.InsertNewTableLog(ipDBVersionDate);
            }

            return Task.FromResult(true);
        }

        private void UpdateIpTableV6(DateTime updatedDate, int fromId, int toId)
        {
            var ipV6Ranges = iPRangeService.GetIpV6Range(fromId, toId);

            if (ipV6Ranges != null && ipV6Ranges.Any())
            {
                var chunkIpV6Range = ipV6Ranges.Chunk(1000);

                Parallel.ForEach(chunkIpV6Range, new ParallelOptions { MaxDegreeOfParallelism = 2 }, x =>
                {
                    iPRangeService.UpdateIpRange(x, updatedDate);
                });

                fromId = (toId + 1);
                toId += batchSize;

                UpdateIpTableV6(updatedDate, fromId, toId);
            }
        }
    }
}

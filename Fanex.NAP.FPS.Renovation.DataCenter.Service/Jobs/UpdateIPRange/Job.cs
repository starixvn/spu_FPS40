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

        public Job()
        {
            iPRangeService = new IPRangeService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            var ipDBVersion = iPRangeService.GetIPDBVersion();
            var ipLatestIPDBLog = iPRangeService.GetLatestUpdatedDateIPTableLog();

            if (ipDBVersion.CompareTo(ipLatestIPDBLog) >= 0)
            {
                UpdateIpTableV4();

                UpdateIpTableV6();

                iPRangeService.InsertNewTableLog(DateTime.Now);
            }

            return Task.FromResult(true);
        }

        private void UpdateIpTableV4()
        {
            var ipV4Ranges = iPRangeService.GetIpV4Range();

            if (ipV4Ranges.Any())
            {
                var chunkIpV4Range = ipV4Ranges.Chunk(100);

                foreach (var range in chunkIpV4Range)
                {
                    iPRangeService.UpdateIpRange(range, 0);
                }
            }
        }

        private void UpdateIpTableV6()
        {
            var ipV6Ranges = iPRangeService.GetIpV6Range();

            if (ipV6Ranges.Any())
            {
                var chunkIpV6Range = ipV6Ranges.Chunk(1000);

                foreach (var range in chunkIpV6Range)
                {
                    iPRangeService.UpdateIpRange(range, 1);
                }
            }
        }
    }
}

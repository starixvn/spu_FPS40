namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Quartz;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly IDataCenterService dataCenterService;

        public Job()
        {
            dataCenterService = new DataCenterService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            UpdateIpTableV4();         

            UpdateIpTableV6();

            return Task.FromResult(true);
        }

        private void UpdateIpTableV4()
        {
            var detectIpTableV4Updated = dataCenterService.GetNewIPTableUpdatedLog(0);

            if(detectIpTableV4Updated.IsNewUpdate)
            {
                var ipV4Ranges = dataCenterService.GetIpV4Range();

                if (ipV4Ranges.Any())
                {
                    var chunkIpV4Range = ipV4Ranges.Chunk(1000);

                    foreach (var range in chunkIpV4Range)
                    {
                        dataCenterService.UpdateIpRange(range.ToList());
                    }

                    dataCenterService.UpdateIpTableLog(0);
                }
            }            
        }

        private void UpdateIpTableV6()
        {
            var detectIpTableV6Updated = dataCenterService.GetNewIPTableUpdatedLog(1);

            if(detectIpTableV6Updated.IsNewUpdate)
            {
                var ipV6Ranges = dataCenterService.GetIpV6Range();

                if (ipV6Ranges.Any())
                {
                    var chunkIpV6Range = ipV6Ranges.Chunk(1000);

                    foreach (var range in chunkIpV6Range)
                    {
                        dataCenterService.UpdateIpRange(range.ToList());
                    }

                    dataCenterService.UpdateIpTableLog(1);
                }
            }           
        }
    }
}

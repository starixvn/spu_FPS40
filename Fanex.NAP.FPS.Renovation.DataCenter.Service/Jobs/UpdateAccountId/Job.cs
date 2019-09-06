namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateAccountId
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Newtonsoft.Json;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataCenterService dataCenter;

        public Job()
        {
            dataCenter = new Services.Implement.DataCenterService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var accountList = dataCenter.GetUpdatedAccount(AppSettings.TransBatch);

                if (accountList.Any())
                {
                    var chunkAccountList = accountList.Chunk(AppSettings.TransBatch / 10);

                    Parallel.ForEach(chunkAccountList, new ParallelOptions { MaxDegreeOfParallelism = 3 }, x =>
                    {
                        var accountJson = JsonConvert.SerializeObject(x.Select(a => new { StagingTransID = a }).ToList());

                        dataCenter.UpdateAccountId(accountJson);
                    });

                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            Console.WriteLine($"Account ID is updated at {DateTime.Now}");

            return Task.FromResult(true);
        }

    }
}

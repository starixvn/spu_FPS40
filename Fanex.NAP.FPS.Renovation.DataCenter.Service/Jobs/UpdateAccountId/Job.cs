namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateAccountId
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Newtonsoft.Json;
    using Quartz;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataCenterService dataCenter;
        private const int numberOfAccount = 1000;

        public Job()
        {
            dataCenter = new Services.Implement.DataCenterService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var accountList = dataCenter.GetUpdatedAccount(numberOfAccount);

                if (accountList.Any())
                {
                    var chunkAccountList = accountList.Chunk(numberOfAccount / 10);

                    var tasks = new List<Task>();

                    foreach (var accounts in chunkAccountList)
                    {
                        var task = Task.Run(() =>
                        {
                            var accountJson = JsonConvert.SerializeObject(accounts.Select(a => new { StagingTransID = a }).ToList());

                            dataCenter.UpdateAccountId(accountJson);
                        });

                        await Task.Delay(10);

                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks.ToArray());

                    Console.WriteLine($"Account ID is updated at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }
        }
    }
}

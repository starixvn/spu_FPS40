namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertTransaction
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using Quartz;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataTransformService dataTransformService;
        
        public Job()
        {
            dataTransformService = new Services.Implement.DataTransformService();
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {                
                var apiUrls = AppSettings.HostPathForTransaction.Split(';').ToList();

                var stagingTranIds = dataTransformService.GetStagingRawTransaction(0, AppSettings.SizeOfTransaction, apiUrls.Count);

                if (stagingTranIds.Any())
                {
                    Core.Utilities.WriteLog("InsertTransaction.log", "Start: " + DateTime.Now);

                    var stagingTrans = stagingTranIds.Select((v, i) =>
                    {
                        v.APIUrl = i > apiUrls.Count ? apiUrls[0] : apiUrls[i];

                        return v;
                    });

                    var tasks = new List<Task>();

                    foreach (var trans in stagingTrans)
                    {
                        var task = Task.Run(() =>
                        {
                            return Executing(trans);
                        });

                        await Task.Delay(10);

                        tasks.Add(task);
                    }

                    await Task.WhenAll(tasks.ToArray());

                    Core.Utilities.WriteLog("InsertTransaction.log", "End: " + DateTime.Now);
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

        private static async Task<IRestResponse> Executing(StagingRawTransactionModel trans)
        {            

            var client = new RestClient(new Uri(trans.APIUrl));
            var request = new RestRequest("api/active", Method.POST);

            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(trans);

            return await client.ExecuteTaskAsync(request);           
        }
    }
}

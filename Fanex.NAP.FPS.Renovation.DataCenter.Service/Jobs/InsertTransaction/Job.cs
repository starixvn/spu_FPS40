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
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataTransformService dataTransformService;
        private const int numberOfRawTrans = 1000;

        public Job()
        {
            dataTransformService = new Services.Implement.DataTransformService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var apiUrls = AppSettings.SelfHostUrl.Split(';').ToList();
                var stagingTranId = dataTransformService.GetStagingRawTransaction(0, numberOfRawTrans);

                if (stagingTranId.MinStagingTransId != 0 && stagingTranId.MaxStagingTransId != 0)
                {
                    var stagingTrans = Enumerable.Range(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId - stagingTranId.MinStagingTransId + 1).ToList();

                    var chunkStagingTrans = stagingTrans.Split(apiUrls.Count);

                    _ = Parallel.ForEach(chunkStagingTrans, new ParallelOptions { MaxDegreeOfParallelism = apiUrls.Count }, (v, s, i) =>
                     {
                         var chunkTrans = v.Chunk(100).ToList();
                         var baseUrl = apiUrls.ElementAt(Convert.ToInt32(i));

                         _ = Parallel.ForEach(chunkTrans, new ParallelOptions { MaxDegreeOfParallelism = 3 }, x =>
                         {
                             var rangeTrans = new StagingRawTransactionModel
                             {
                                 MinStagingTransId = x.Min(),
                                 MaxStagingTransId = x.Max()
                             };

                             var client = new RestClient(new Uri(baseUrl));
                             var request = new RestRequest("api/active", Method.POST);

                             request.AddHeader("Content-type", "application/json");
                             request.AddJsonBody(rangeTrans);

                             var response = client.Execute(request);

                             if (response.ResponseStatus != ResponseStatus.Completed)
                             {
                                 LogService.Error(baseUrl + " - " + response.ErrorException.Message);
                             }
                         });
                     }
                     );
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(true);
        }
    }
}

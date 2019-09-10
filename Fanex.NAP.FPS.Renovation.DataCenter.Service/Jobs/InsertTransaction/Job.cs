namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertTransaction
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using Newtonsoft.Json;
    using Quartz;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataTransformService dataTransformService;
        private const int numberOfRawTrans = 1000;
        private const int chunkOfTrans = numberOfRawTrans / 10;
        public Job()
        {
            dataTransformService = new Services.Implement.DataTransformService();


        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var apiUrls = AppSettings.SelfHostUrl.Split(';').ToList();
                var index = 0;
                var stagingTranId = dataTransformService.GetStagingRawTransaction(0, numberOfRawTrans);

                if (stagingTranId.MinStagingTransId != 0 && stagingTranId.MaxStagingTransId != 0)
                {                   
                    var stagingTrans = Enumerable.Range(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId).ToList();

                    var chunkStagingTrans = stagingTrans.Chunk(chunkOfTrans);

                    _ = Parallel.ForEach(chunkStagingTrans, new ParallelOptions { MaxDegreeOfParallelism = apiUrls.Count }, x =>
                      {
                          var rangeTrans = new StagingRawTransactionModel
                          {
                              MinStagingTransId = x.Min(),
                              MaxStagingTransId = x.Max()
                          };
                          
                          var baseUrl = apiUrls.ElementAt(index >= apiUrls.Count ? 0 : index);
                          var client = new RestClient(new Uri(baseUrl));
                          var request = new RestRequest("api/active", Method.POST);

                          request.AddHeader("Content-type", "application/json");
                          request.AddJsonBody(rangeTrans);
                          client.Execute(request);

                          if (index >= apiUrls.Count)
                          {
                              index = 0;
                          }
                          else
                          {
                              index++;
                          }
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

            return Task.FromResult(true);
        }
    }
}

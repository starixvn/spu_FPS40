namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.InsertDevice
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using MySql.Data.MySqlClient;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IDataCenterService dataCenterService;
        private const int numberOfTrans = 1000;

        public Job()
        {
            dataCenterService = new Services.Implement.DataCenterService();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                //var trans = dataCenterService.GetStagingTransIdForDevice(numberOfTrans);

                //if (trans.Any())
                //{
                //    var transGroup = trans.Chunk(numberOfTrans / 10);

                //    Parallel.ForEach(transGroup, new ParallelOptions { MaxDegreeOfParallelism = 3 }, x =>
                //    {
                //        dataCenterService.InsertDevice(x.ToList());
                //    });
                //}
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex.InnerException).Number != 1213)
                {
                    var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                    LogService.Error(logMessage);
                }
                    
            } 

            Console.WriteLine($"Device is inserted at {DateTime.Now}");

            return Task.FromResult(true);
        }
    }
}

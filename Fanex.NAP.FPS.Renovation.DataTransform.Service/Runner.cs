namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
    using Fanex.NAP.FPS.Renovation.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Threading.Tasks;
    using System.Timers;

    public partial class Runner : ServiceBase
    {
        private readonly IDataTransformService dataTransformService;
        private System.Timers.Timer timer;
        private bool isRunning;
        public Runner()
        {
            dataTransformService = new DataTransformService();
            isRunning = false;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimedEvent;
            timer.Interval = AppSettings.Interval;
            timer.Enabled = true;

            LogService.Info($"Data Transform service is STARTED at {DateTime.Now}");
        }

        protected override void OnStop()
        {
            timer.Dispose();

            LogService.Info($"Data Transform service is STOPED at {DateTime.Now}");
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var insertResult = new DataResult();

            try
            {
                if (isRunning)
                {
                    return;
                }

                isRunning = true;

                var stagingTranId = dataTransformService.GetStagingRawTransaction(0, AppSettings.TransBatch);

                if (stagingTranId != null)
                {
                    var chunkSize = AppSettings.TransBatch / 10;

                    var tranIds = Enumerable.Range(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId).ToList();                    

                    var chunkTransIds = tranIds.OrderBy(x => x).Chunk(chunkSize);

                    var option = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 5
                    };

                    Parallel.ForEach(chunkTransIds, option, transId => {

                        var min = transId.Min();
                        var max = transId.Max();

                        dataTransformService.InsertAccounts(min, max);                         
                    });
                }

                isRunning = false;
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex))
                    .ToString();

                LogService.Error(logMessage);

                isRunning = false;
            }
        }
    }
}

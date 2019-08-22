namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
    using System;
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
            Core.Utilities.WriteLog("RawTrans2DataCenter.txt", "Start - " +  DateTime.Now.ToString());

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
            try
            {
                if (isRunning)
                {
                    return;
                }

                isRunning = true;

                var stagingTranId = dataTransformService.GetStagingRawTransaction(0, AppSettings.TransBatch);               

                if (stagingTranId.MinStagingTransId  != 0 && stagingTranId.MaxStagingTransId != 0)
                {
                    dataTransformService.UnhandleSubscriberAccountProcessing(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId);

                    Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 2 },() =>
                    {
                        dataTransformService.AccountProcessing(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId);                        
                    }, () =>
                    {
                        dataTransformService.UserAgentProcessing(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId);
                    });

                    dataTransformService.DeviceProcessing(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId);

                    dataTransformService.TransactionProcessing(stagingTranId.MinStagingTransId, stagingTranId.MaxStagingTransId);
                }
                else
                {
                    Core.Utilities.WriteLog("RawTrans2DataCenter.txt", "End - " + DateTime.Now.ToString());

                    timer.Stop();
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

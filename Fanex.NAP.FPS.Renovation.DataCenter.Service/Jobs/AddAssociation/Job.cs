namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.AddAssociation
{
    using EasyNetQ;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Fanex.NAP.FPS.Renovation.Model;
    using Fanex.NAP.FPS.Renovation.Queue;
    using Quartz;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly IDataCenterService dataCenterService;
        private readonly EasyNetQConnector _rabbitMQ;
        private const int numberOfAssociation = 1000;

        public Job()
        {
            dataCenterService = new DataCenterService();

            _rabbitMQ = new EasyNetQConnector(AppSettings.Host + ":" + AppSettings.Post,
                AppSettings.VirtualHost,
                AppSettings.PrefetchCount,
                AppSettings.UserName,
                AppSettings.Password);

            _rabbitMQ.CreateBus()
                .DeclareExchange(AppSettings.Exchange)
                .DeclareQueue(AppSettings.QueueName)
                .DeclareRoutingKey(AppSettings.RoutingKey)
                .Bind();
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var associations = dataCenterService.GetAssociationForGrapDB(0, numberOfAssociation);

                if (associations != null && associations.Any())
                {
                    Core.Utilities.WriteLog("AddAssociation.log", "Start: " + DateTime.Now);

                    foreach (var association in associations)
                    {
                        var messageQueue = new Message<AssociationModel>(association, new MessageProperties { DeliveryMode = 2, Priority = 9 });

                        _rabbitMQ.AdvanceBus.PublishAsync(_rabbitMQ.Exchange, _rabbitMQ.RoutingKey, false, messageQueue);
                    }

                    var isSynAssociation = associations.Select(x => new IsSynAssociationModel
                    {
                        AssociationID = x.AssociationId,
                        SyncGraphDB = 1
                    }).ToList();

                    var xmlData = Core.Utilities.XMLConvert(isSynAssociation);

                    dataCenterService.UpdateIsSyncByAssociationId(xmlData);

                    Core.Utilities.WriteLog("AddAssociation.log", "End: " + DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            Console.WriteLine($"Association is updated at {DateTime.Now}");

            return Task.FromResult(true);
        }
    }
}

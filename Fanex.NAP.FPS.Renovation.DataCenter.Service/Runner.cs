using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using EasyNetQ;
using Fanex.Data;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.Queue;
using Quartz.Util;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service
{
    public partial class Runner : ServiceBase
    {
        //private static IDataCenterService dataCenterService;
        //private static Timer timer;
        //private static bool isRunning;
        //private static EasyNetQConnector _rabbitMQ;
        private static List<ISchedule> schedulers;
        public Runner()
        {
            //dataCenterService = new DataCenterService();

            //_rabbitMQ = new EasyNetQConnector(AppSettings.Host + ":" + AppSettings.Post,
            //    AppSettings.VirtualHost,
            //    AppSettings.PrefetchCount,
            //    AppSettings.UserName,
            //    AppSettings.Password);

            //_rabbitMQ.CreateBus()
            //    .DeclareExchange(AppSettings.Exchange)
            //    .DeclareQueue(AppSettings.QueueName)
            //    .DeclareRoutingKey(AppSettings.RoutingKey)
            //    .Bind();

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            schedulers = new List<ISchedule>();

            var assembly = typeof(Program).GetTypeInfo().Assembly;
            var types = assembly.GetTypes();            

            foreach (Type type in types)
            {
                if (new List<Type>(type.GetInterfaces()).Contains(typeof(ISchedule)))
                {
                    var scheduler = ObjectUtils.InstantiateType<ISchedule>(type);

                    scheduler.Run();

                    schedulers.Add(scheduler);
                }
            }

            LogService.Info($"Data Center service is STARTED at {DateTime.Now}");
        }

        protected override void OnStop()
        {
            schedulers.ForEach(x => x.Shutdown());

            LogService.Info($"Data Center service is STOPED at {DateTime.Now}");
        }

        //private void OnTimedEvent(object source, ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        if (isRunning) return;

        //        isRunning = true;

        //        var newCustomer = dataCenterService.GetNewCustomer(AppSettings.TransBatch);

        //        if (newCustomer != null && newCustomer.Any())
        //        {
        //            var customerInfo = dataCenterService.GetCustomerInfo(newCustomer);

        //            if (customerInfo != null && customerInfo.Any())
        //            {
        //                var result = dataCenterService.UpdateCustomerInfo(customerInfo);

        //                //if (result.ErrorCode == 0)
        //                //{
        //                //    foreach (var customer in customerInfo)
        //                //    {
        //                //        var associationCustomer = TransformCustomerInfo(customer);

        //                //        var messageQueue = new Message<AssociationModel>(associationCustomer, new MessageProperties() { DeliveryMode = 2, Priority = 9 });

        //                //        _rabbitMQ.AdvanceBus.PublishAsync(_rabbitMQ.Exchange, _rabbitMQ.RoutingKey, false, messageQueue);
        //                //    }
        //                //}
        //            }
        //        }

        //        isRunning = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        var logMessage = new LogMessage(ex.Message)                    
        //            .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

        //        LogService.Error(logMessage);

        //        isRunning = false;
        //    }
        //}

        //private AssociationModel TransformCustomerInfo(CustomerInfoModel customerInfo)
        //{
        //    return new AssociationModel()
        //    {
        //        CreatedDate = customerInfo.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss"),
        //        CustId = customerInfo.CustId,
        //        Username2 = customerInfo.Username2,
        //        Username = customerInfo.Username,
        //        RoleId = customerInfo.RoleId,
        //        DeviceId = 0
        //    };
        //}
    }
}

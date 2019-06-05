using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
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

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service
{
    public partial class Runner : ServiceBase
    {
        private static IDataCenterService dataCenterService;
        private static Timer timer;
        private static bool isRunning;
        private static EasyNetQConnector _rabbitMQ;

        public Runner()
        {
            dataCenterService = new DataCenterService();

            _rabbitMQ = new EasyNetQConnector(AppSetting.Host + ":" + AppSetting.Post,
                AppSetting.VirtualHost,
                AppSetting.PrefetchCount,
                AppSetting.UserName,
                AppSetting.Password);

            _rabbitMQ.CreateBus()
                .DeclareExchange(AppSetting.Exchange)
                .DeclareQueue(AppSetting.QueueName)
                .DeclareRoutingKey(AppSetting.RoutingKey)
                .Bind();

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();

            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 10000;
            timer.Enabled = true;
            
        }

        protected override void OnStop()
        {
            timer.Dispose();
            _rabbitMQ.Dispose();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (isRunning) return;

                isRunning = true;

                var newCustomer = dataCenterService.GetNewCustomer();

                if (newCustomer != null && newCustomer.Any())
                {
                    var customerInfo = dataCenterService.GetCustomerInfo(newCustomer);

                    if (customerInfo != null && customerInfo.Any())
                    {
                        var result = dataCenterService.UpdateCustomerInfo(customerInfo);

                        if (result.ErrorCode == 0)
                        {
                            foreach (var customer in customerInfo)
                            {
                                var associationCustomer = TransformCustomerInfo(customer);

                                var messageQueue = new Message<AssociationCustomer>(associationCustomer, new MessageProperties() { DeliveryMode = 2, Priority = 9 });

                                _rabbitMQ.AdvanceBus.PublishAsync(_rabbitMQ.Exchange, _rabbitMQ.RoutingKey, false, messageQueue);
                            }
                        }
                    }
                }

                isRunning = false;
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(ex.Message);
            }
        }

        private AssociationCustomer TransformCustomerInfo(CustomerInfo customerInfo)
        {
            return new AssociationCustomer()
            {
                CreatedDate = customerInfo.CreatedDate.ToString("yyyy-MM-dd hh:mm:ss"),
                CustId = customerInfo.CustId,
                Username2 = customerInfo.Username2,
                Username = customerInfo.Username,
                RoleId = customerInfo.RoleId,
                DeviceId = 0
            };
        }
    }
}

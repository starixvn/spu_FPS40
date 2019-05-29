using Fanex.Graph.Data;
using Fanex.NAP.FPS.Renovation.Criteria.GraphDB;
using Fanex.NAP.FPS.Renovation.Queue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.GraphDB.Service
{
    public partial class Runner : ServiceBase
    {
        private EasyNetQConnector easyNetQService;

        public Runner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            easyNetQService = new EasyNetQConnector(AppSettings.RabbitMQ_Host,
                AppSettings.RabbitMQ_VirtualHost,
                Convert.ToInt32(AppSettings.RabbitMQ_PrefetchCount),
                AppSettings.RabbitMQ_Username,
                AppSettings.RabbitMQ_Password).CreateBus();
            easyNetQService.DeclareQueue(AppSettings.RabbitMQ_Queue)
                            .DeclareExchange(AppSettings.RabbitMQ_Exchange)
                            .DeclareRoutingKey(AppSettings.RabbitMQ_RoutingKey)
                            .Bind();

            AddHandlerForGetMassage(InsertCustomer);
        }

        protected override void OnStop()
        {
            easyNetQService.Dispose();
        }

        public void AddHandlerForGetMassage(Action<AddCustomerAndAssociationCriteria> handler)
        {
            easyNetQService.AdvanceBus.Consume(easyNetQService.Queue,
                registration => registration.Add<AddCustomerAndAssociationCriteria>((message, info) =>
                {
                    handler(message.Body);
                }));
        }
        private void InsertCustomer(AddCustomerAndAssociationCriteria criteria)
        {
            // ko luu SMA, chi luu member, roleid = 1
            if (criteria.RoleId == 1)
            {
                // remove test account
                if (!criteria.Username.Contains("test") || criteria.Username2 == null || !criteria.Username2.Contains("test"))
                {
                    try
                    {
                        var custId = DbSessionFactory.Query<int>(FPS.GetCustomerById, new { criteria.CustId });
                        var deviceId = DbSessionFactory.Query<int>(FPS.GetDeviceById, new { criteria.DeviceId });
                        var association = DbSessionFactory.Query<int>(FPS.CheckExistedAssociation, new { criteria.CustId, criteria.DeviceId });
                        if (criteria.DeviceId == 0)
                        {
                            if (!custId.Any())
                            {
                                DbSessionFactory.Execute(FPS.AddNewCustomer, criteria);
                            }
                        }
                        else
                        {
                            DbSessionFactory.Execute(FPS.AddAssociation, criteria);

                            if (custId.Any())
                            {
                                if (deviceId.Any())
                                {
                                    // case acc cu, device cu
                                    if (association.Any())
                                    {
                                        DbSessionFactory.Execute(FPS.UpdateCustomerUsername2, new { criteria.CustId, criteria.Username2 });
                                    }
                                    else
                                    {
                                        DbSessionFactory.Execute(FPS.MergeGroup, new { criteria.CustId, criteria.DeviceId });
                                    }
                                }
                            }
                            else
                            {
                                if (deviceId.Any())
                                {
                                    // case acc moi, device cu
                                    DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, criteria.DeviceId });
                                }
                                else
                                {
                                    // case acc moi, device moi
                                    DbSessionFactory.Execute(FPS.CreateNewGroup, new { criteria.CustId });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //Log error;
                    }
                }
            }
        }
    }
}

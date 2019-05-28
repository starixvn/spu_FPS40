using Fanex.Graph.Data;
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
        public Runner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        //private void InsertCustomer(AddCustomerAndAssociationCriteria criteria)
        //{
        //    // ko luu SMA, chi luu member, roleid = 1
        //    if (criteria.RoleId == 1)
        //    {
        //        // remove test account
        //        if (!criteria.Username.Contains("test") || criteria.Username2 == null || !criteria.Username2.Contains("test"))
        //        {
        //            try
        //            {
        //                var custId = DbSessionFactory.Query<int>(FPS.GetCustomerById, new { criteria.CustId });
        //                var deviceId = DbSessionFactory.Query<int>(FPS.GetDeviceById, new { criteria.DeviceId });
        //                var association = DbSessionFactory.Query<int>(FPS.CheckExistedAssociation, new { criteria.CustId, criteria.DeviceId });
        //                if (criteria.DeviceId == 0)
        //                {
        //                    if (!custId.Any())
        //                    {
        //                        DbSessionFactory.Execute(FPS.AddNewCustomer, criteria);
        //                    }
        //                }
        //                else
        //                {
        //                    DbSessionFactory.Execute(FPS.AddAssociation, criteria);

        //                    if (custId.Any())
        //                    {
        //                        if (deviceId.Any())
        //                        {
        //                            // case acc cu, device cu
        //                            if (association.Any())
        //                            {
        //                                DbSessionFactory.Execute(FPS.UpdateCustomerUsername2, new { criteria.CustId, criteria.Username2 });
        //                            }
        //                            else
        //                            {
        //                                DbSessionFactory.Execute(FPS.MergeGroup, new { criteria.CustId, criteria.DeviceId });
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (deviceId.Any())
        //                        {
        //                            // case acc moi, device cu
        //                            DbSessionFactory.Execute(FPS.AddCustomerGroupRelationship, new { criteria.CustId, criteria.DeviceId });
        //                        }
        //                        else
        //                        {
        //                            // case acc moi, device moi
        //                            DbSessionFactory.Execute(FPS.CreateNewGroup, new { criteria.CustId });
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                //Log error;
        //            }
        //        }
        //    }
        //}
    }
}

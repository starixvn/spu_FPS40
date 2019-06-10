using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Criteria;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    public class DataCenter
    {
        public DataResult InsertTransaction(string xmlTrans)
        {
            var repository = new DynamicRepository();
            
            var criteria = new InsertDataCenterTransactionCriteria()
            {
                StagingTransXml = xmlTrans,
                ErrCode = 0,
                ErrMessage = String.Empty
            };

            repository.Execute(criteria);

            return new DataResult()
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMessage
            };
        }

        public List<Model.NewCustomer> GetNewCustomer(int size)
        {
            var repository = new DynamicRepository();

            var criteria = new GetNewCustomerCriteria()
            {
                Size = size
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<Model.NewCustomer>(criteria).ToList();
            }
        }

        public DataResult UpdateCustomerInfo(string custDataXml)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateCustomerInfoCriteria()
            {
                CustDataXml = custDataXml,
                ErrCode = 0,
                ErrMessage = String.Empty
            };

            repository.Execute(criteria);

            return new DataResult()
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMessage
            };
        }

        public List<Model.CustomerInfo> GetCustomerInfo(string newCustomers)
        {
            var repository = new DynamicRepository();

            var criteria = new GetCustomerInfoCriteria()
            {
                CustomerList = newCustomers
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<Model.CustomerInfo>(criteria).ToList();
            }
        }
    }
}

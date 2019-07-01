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
    public class DataCenterDataAccess
    {
        public DataResult InsertTransaction(string xmlTrans)
        {
            var repository = new DynamicRepository();

            var criteria = new InsertDataCenterTransactionCriteria
            {
                StagingTransXml = xmlTrans
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public List<NewCustomerModel> GetNewCustomer(int size)
        {           
            var criteria = new GetNewCustomerCriteria
            {
                Size = size
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<NewCustomerModel>(criteria).ToList();
            }
        }

        public DataResult UpdateCustomerInfo(string custDataXml)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateCustomerInfoCriteria
            {
                CustDataXml = custDataXml
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public List<Model.CustomerInfoModel> GetCustomerInfo(string newCustomers)
        {
            var criteria = new GetCustomerInfoCriteria
            {
                CustomerList = newCustomers
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<CustomerInfoModel>(criteria).ToList();
            }
        }

        public List<Model.AssociationModel> GetAssociation(int isSysnGraphDB, int size)
        {

            var criteria = new GetAssociationForGraphDBCriteria
            {
                SyncGraphDB = isSysnGraphDB,
                Size = size
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AssociationModel>(criteria).ToList();
            }
        }

        public DataResult UpdateIsSyncGraphDBByAssociationId(string associationXml)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateIsSyncGraphDBByAssociationIdCriteria
            {
                AssociationXml = associationXml
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult UpdateIPRange(string ipRangeXml)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateIpRangeCriteria
            {
                IPRangeLocationXml = ipRangeXml
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public List<IpRangeModel> GetIpV4Range()
        {
            var criteria = new GetIpV4RangeCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<IpRangeModel>(criteria).ToList();
            }
        }

        public List<IpRangeModel> GetIpV6Range()
        {
            var criteria = new GetIpV6RangeCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<IpRangeModel>(criteria).ToList();
            }
        }

        public NewIPTableUpdatedModel GetNewIPTableUpdatedLog(int ipType)
        {
            var criteria = new GetNewIPTableUpdatedLogCriteria
            {
                IpType = ipType
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<NewIPTableUpdatedModel>(criteria);
            }
        }

        public DataResult UpdateIpTableLog(int ipType)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateIPTableLogCriteria
            {
                IPType = ipType
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }
    }
}

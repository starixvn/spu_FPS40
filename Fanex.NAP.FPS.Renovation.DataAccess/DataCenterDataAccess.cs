namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.FPS.Renovation.Criteria;
    using Fanex.NAP.FPS.Renovation.Model;
    using System.Collections.Generic;
    using System.Linq;

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

        public List<AccountExistedModel> AccountProcess(int fromId, int toId)
        {
            var criteria = new ProcessAccountCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AccountExistedModel>(criteria).ToList();
            }
        }

        public DataResult InsertRawAccount(string jsonAccounts)
        {
            var repository = new DynamicRepository();

            var criteria = new InsertRawAccountCriteria
            {
                AccountJson = jsonAccounts
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult UserAgentProcess(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new ProcessUserAgentCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult TransactionProcess(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new ProcessTransactionCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult CompleteProcess(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new ProcessCompletedCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }


        public DataResult UpdateTransactionDevice(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateTransactionDeviceCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult UpdateTransactionAccount(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateTransactionAccountCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult DeviceProcess(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new ProcessDeviceCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public DataResult UnhandleSubscriberActionProcess(int fromId, int toId)
        {
            var repository = new DynamicRepository();

            var criteria = new ProcessUnhandleSubscriberActionCriteria
            {
                FromStagingTransId = fromId,
                ToStagingTransId = toId
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }

        public List<UserAgentModel> GetUserAgent(int size)
        {
            var criteria = new GetUserAgentCriteria
            {
               Size = size
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<UserAgentModel>(criteria).ToList();
            }
        }                
                
        public DataResult UpdateUserAgent(string userAgentJson)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateUserAgentCriteria
            {
                UserAgentJson = userAgentJson
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }
    }
}

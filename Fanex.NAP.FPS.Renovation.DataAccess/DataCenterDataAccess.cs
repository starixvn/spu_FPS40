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

        public DataResult InsertRawAccount(string xmlAccounts)
        {
            var repository = new DynamicRepository();

            var criteria = new InsertRawAccountCriteria
            {
                AccountXml = xmlAccounts
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
                fromStagingTransId = fromId,
                toStagingTransId = toId
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
                fromStagingTransId = fromId,
                toStagingTransId = toId
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
                fromStagingTransId = fromId,
                toStagingTransId = toId
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

        public List<BrowserModel> GetExistingBrowser(string browserXML)
        {
            var criteria = new GetExistingBrowserCriteria
            {
                BrowserXML = browserXML
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<BrowserModel>(criteria).ToList();
            }
        }

        public List<OSModel> GetExistingOS(string osXML)
        {
            var criteria = new GetExistingOSCriteria
            {
                OSXML = osXML
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<OSModel>(criteria).ToList();
            }
        }

        public int InsertBrowser( string browserName)
        {
            var repository = new DynamicRepository();

            var criteria = new InsertBrowserCriteria
            {
                BrowserId = 0,
                BrowserName = browserName
            };

            repository.Execute(criteria);

            return criteria.BrowserId;
        }

        public int InsertOS(string osName)
        {
            var repository = new DynamicRepository();

            var criteria = new InsertOSCriteria
            {
                OSId = 0,
                OSName = osName
            };

            repository.Execute(criteria);

            return criteria.OSId;
        }

        public DataResult UpdateUserAgent(string userAgentXML)
        {
            var repository = new DynamicRepository();

            var criteria = new UpdateUserAgentCriteria
            {
                UserAgentXML = userAgentXML
            };

            var errCode = repository.Execute(criteria);

            return new DataResult
            {
                ErrorCode = errCode
            };
        }
    }
}

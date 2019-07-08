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
    }
}

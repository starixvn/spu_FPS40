using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Criteria;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    public class AssociationDataAccess
    {
        public List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(ICriteria criteria)
        {
            var repository = new DynamicRepository();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<TotalAssociationByCustomerModel>(criteria).ToList();
            }
        }

        public List<AssociationByCustomerModel> GetAssociationByCustomer(ICriteria criteria)
        {
            var repository = new DynamicRepository();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AssociationByCustomerModel>(criteria).ToList();
            }
        }

        public List<CustomerEvidence> GetCustomersEvidences(ICriteria criteria)
        {
            var repository = new DynamicRepository();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<CustomerEvidence>(criteria).ToList();
            }
        }
    }
}

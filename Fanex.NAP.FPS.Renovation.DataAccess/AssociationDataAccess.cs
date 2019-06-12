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
    public class AssociationDataAccess
    {
        public List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(string custIds)
        {
            var repository = new DynamicRepository();

            var criteria = new GetTotalAssociationByCustomerCriteria()
            {
              CustIds = custIds
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<TotalAssociationByCustomerModel>(criteria).ToList();
            }
        }

        public List<AssociationByCustomerModel> GetAssociationByCustomer(string custIds)
        {
            var repository = new DynamicRepository();

            var criteria = new GetAssociationByCustomerCriteria()
            {
                CustIds = custIds
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AssociationByCustomerModel>(criteria).ToList();
            }
        }
    }
}

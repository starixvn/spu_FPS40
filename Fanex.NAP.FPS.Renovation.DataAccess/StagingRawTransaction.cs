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
    public class StagingRawTransaction
    {
        public List<Model.StagingRawTransaction> GetStagingRawTrans()
        {
            var repository = new DynamicRepository();

            var criteria = new GetStagingRawTransactionCriteria();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<Model.StagingRawTransaction>(criteria).ToList();
            }
        }
    }
}

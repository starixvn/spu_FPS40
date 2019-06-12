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
    public class StagingRawTransactionDataAccess
    {
        public List<Model.StagingRawTransactionModel> GetStagingRawTrans(int isProcessed = 0)
        {
            var repository = new DynamicRepository();

            var criteria = new GetRawTransactionCriteria()
            {
                IsProcessed = isProcessed
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<Model.StagingRawTransactionModel>(criteria).ToList();
            }
        }
    }
}

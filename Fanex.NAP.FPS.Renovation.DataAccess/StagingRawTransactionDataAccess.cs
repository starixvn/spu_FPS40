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
        public StagingRawTransactionModel GetStagingRawTrans(int isProcessed = 0, int numberOfTicket = 100 )
        {
            var criteria = new GetRawTransactionCriteria
            {
                IsProcessed = isProcessed,
                NumberOfTickets = numberOfTicket
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<StagingRawTransactionModel>(criteria);
            }
        }

        public List<AccountExistedModel> AccountProcess(int fromId, int toId)
        {
            var criteria = new RawTransProcessAccountCriteria
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
    }
}

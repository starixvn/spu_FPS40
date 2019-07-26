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
        public List<StagingRawTransactionModel> GetStagingRawTrans(int isProcessed = 0, int numberOfTicket = 100 )
        {
            var criteria = new GetRawTransactionCriteria
            {
                IsProcessed = isProcessed,
                NumberOfTickets = numberOfTicket
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<StagingRawTransactionModel>(criteria).ToList();
            }
        }

        public List<AccountExistedModel> GetAccountExist(string accountListXML)
        {
            var criteria = new GetExistingAccount
            {
                AccountListXML = accountListXML
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<AccountExistedModel>(criteria).ToList();
            }
        }
    }
}

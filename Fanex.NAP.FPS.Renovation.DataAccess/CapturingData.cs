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
    public class CapturingData
    {
        public void InsertTransaction(RawTransactionModel model)
        {
            var repository = new DynamicRepository();

            var criteria = new CapturingTransactionCriteria()
            {
                Account = model.Account,
                Action = model.Action,
                ActionResult = model.ActionResult,
                CaptchaResult = model.CaptchaResult,
                Device =  model.Device,
                Ip = model.IP,
                Subscriber = model.Subscriber,
                SubscriberDate = model.SubscriberDate,
                Token = model.Token
            };

            repository.Execute(criteria);
        }
    }
}

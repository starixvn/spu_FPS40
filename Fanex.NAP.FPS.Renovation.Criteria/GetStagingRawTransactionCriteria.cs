using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetStagingRawTransactionCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "RT_StagingRawTransaction_GetProcessedTransList";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateAccountIdCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_AccountIDJson")]
        public string AccountIdJson { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_DataCenter_Process_UpdateAccountID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

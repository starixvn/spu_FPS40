using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetExistingAccount : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_AccountListXML")]
        public string AccountListXML { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_CheckAccountExist";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

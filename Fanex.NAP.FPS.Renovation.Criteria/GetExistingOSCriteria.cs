using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetExistingOSCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_OSXML")]
        public string OSXML { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_CheckOSExist";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

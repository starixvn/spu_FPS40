using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetExistingBrowserCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ip_BrowserXML")]
        public string BrowserXML { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_CheckBrowserExist";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

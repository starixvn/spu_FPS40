using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateUserAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "in_UserAgentXML")]
        public string UserAgentXML { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_UpdateUserAgent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

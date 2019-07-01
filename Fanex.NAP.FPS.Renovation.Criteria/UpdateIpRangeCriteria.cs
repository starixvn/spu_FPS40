using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateIpRangeCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_IPRangeLocationXml")]
        public string IPRangeLocationXml { get; set; }

        public override string GetSettingKey()
        {
            return "DC_UpdateIPRangeLocationByXML";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

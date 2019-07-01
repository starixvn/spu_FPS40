using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateIPTableLogCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_IPType")]
        public int IPType { get; set; }

        public override string GetSettingKey()
        {
            return "DC_UpdateIPTableLog";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

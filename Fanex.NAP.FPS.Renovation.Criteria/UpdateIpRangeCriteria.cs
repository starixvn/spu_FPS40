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
        [SpParam(SqlParamName = "ip_UpdatedDate")]
        public DateTime UpdatedDate { get; set; }

        [SpParam(SqlParamName = "ip_IsInitial")]
        public int IsInitial { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_IPRangeLocation_AddNew";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

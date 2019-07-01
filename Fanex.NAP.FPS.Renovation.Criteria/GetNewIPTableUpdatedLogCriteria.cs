using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetNewIPTableUpdatedLogCriteria : CriteriaBase
    {
        public int IpType { get; set; }

        public override string GetSettingKey()
        {
            return "GetIPTableLogNew";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

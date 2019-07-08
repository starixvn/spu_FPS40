using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class GetIpV4RangeCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "GetAllIP2Location";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class InsertOSCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "in_OS")]
        public string OSName { get; set; }

        [SpParam(SqlParamName = "out_OSID")]
        public int OSId { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_InsertOS";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

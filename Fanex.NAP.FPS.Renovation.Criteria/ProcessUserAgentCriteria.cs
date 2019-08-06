using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class ProcessUserAgentCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_FromStagingTransID")]
        public int fromStagingTransId { get; set; }

        [SpParam(SqlParamName = "ip_ToStagingTransID")]
        public int toStagingTransId { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_DataCenter_Process_UserAgent";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

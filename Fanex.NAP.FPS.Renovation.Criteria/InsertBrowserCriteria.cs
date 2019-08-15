using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class InsertBrowserCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "in_Browser")]
        public string BrowserName { get; set; }

        [SpParam(SqlParamName = "out_BrowserID")]
        public int BrowserId { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_InsertBrowser";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

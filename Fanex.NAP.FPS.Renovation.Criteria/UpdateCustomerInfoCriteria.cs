using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateCustomerInfoCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "custDataXml")]
        public string CustDataXml { get; set; }

        [SpParam(SqlParamName = "op_ErrorCode")]
        public int ErrCode { get; set; }

        [SpParam(SqlParamName = "op_ErrorMessage")]
        public string ErrMessage { get; set; }

        public override string GetSettingKey()
        {
            return "DC_UpdateCustomerInfo";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

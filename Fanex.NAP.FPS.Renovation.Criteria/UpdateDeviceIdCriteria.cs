using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateDeviceIdCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_DeviceIDJson")]
        public string DeviceIdJson { get; set; }

        public override string GetSettingKey()
        {
            return "FPS_DC_TransformData_Transaction_UpdateDeviceID";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

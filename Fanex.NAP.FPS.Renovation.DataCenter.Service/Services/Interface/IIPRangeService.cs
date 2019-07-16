using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
{
    interface IIPRangeService
    {
        DataResult UpdateIpRange(IEnumerable<IpRangeModel> IpRanges, DateTime updatedDate);

        List<IpRangeModel> GetIpV6Range(int fromId, int toId);

        DateTime GetIPDBVersion();

        DateTime GetLatestUpdatedDateIPTableLog();

        DataResult InsertNewTableLog(DateTime updatedDate);
    }
}

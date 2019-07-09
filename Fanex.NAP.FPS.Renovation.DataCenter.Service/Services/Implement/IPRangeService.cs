using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement
{
    public class IPRangeService : IIPRangeService
    {
        private readonly DataAccess.IPRangeDataAccess ipRangeDataAccess;

        public IPRangeService()
        {
            ipRangeDataAccess = new IPRangeDataAccess();
        }

        public DataResult UpdateIpRange(IEnumerable<IpRangeModel> IpRanges, int ipType)
        {
            var ipRangeEncode = IpRanges.Select(x => {
                x.IPRangeLocationCode = EncodeIPAttribute(x);
                x.IPType = ipType;
                return x;
            }).ToList();

            var ipRangeXml = Utilities.XMLConvert(ipRangeEncode);

            return ipRangeDataAccess.UpdateIPRange(ipRangeXml: ipRangeXml);
        }

        public List<IpRangeModel> GetIpV4Range() => ipRangeDataAccess.GetIpV4Range();

        public List<IpRangeModel> GetIpV6Range() => ipRangeDataAccess.GetIpV6Range();

        public DateTime GetIPDBVersion() => ipRangeDataAccess.GetIPDBVersion();        

        public DateTime GetLatestUpdatedDateIPTableLog() => ipRangeDataAccess.GetLatestUpdatedDataIPTableLog();
        
        public DataResult InsertNewTableLog(DateTime updatedDate)
        {
            return ipRangeDataAccess.InsertNewTableLog(updatedDate);
        }

        private static string EncodeIPAttribute(IpRangeModel ipRangeModel)
        {
            return Cipher.Hashing(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}", 
                ipRangeModel.IPType,
                ipRangeModel.FromIP,
                ipRangeModel.ToIP, 
                ipRangeModel.CountryCode, 
                ipRangeModel.CountryName, 
                ipRangeModel.Region, 
                ipRangeModel.City, 
                ipRangeModel.ISPName));
        }
    }
}

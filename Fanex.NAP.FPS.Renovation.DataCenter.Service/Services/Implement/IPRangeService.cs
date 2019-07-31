namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataAccess;
    using Fanex.NAP.FPS.Renovation.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class IPRangeService : Interface.IPRangeService
    {
        private readonly DataAccess.IPRangeDataAccess ipRangeDataAccess;

        public IPRangeService()
        {
            ipRangeDataAccess = new IPRangeDataAccess();
        }

        public int UpdateIpRange(IEnumerable<IpRangeModel> IpRanges, DateTime updatedDate)
        {
            var ipRangeEncode = IpRanges.Select(x => {
                x.IPRangeLocationCode = EncodeIPAttribute(x);
                x.FromIP = x.FromIP.Trim();
                x.ToIP = x.ToIP.Trim();
                return x;
            }).ToList();            

            var ipRangeXml = Utilities.XMLConvert(ipRangeEncode);

            var result = ipRangeDataAccess.UpdateIPRange(ipRangeXml, updatedDate);

            return result;
        }        

        public DateTime GetIPDBVersion() => ipRangeDataAccess.GetIPDBVersion();        

        public DateTime GetLatestUpdatedDateIPTableLog() => ipRangeDataAccess.GetLatestUpdatedDataIPTableLog();
        
        public DataResult InsertNewTableLog(DateTime updatedDate)
        {
            return ipRangeDataAccess.InsertNewTableLog(updatedDate);
        }

        private static int EncodeIPAttribute(IpRangeModel ipRangeModel)
        {            
            return Cipher.Hashing2Int(string.Format("{0}{1}{2}{3}{4}{5}{6}",
                ipRangeModel.FromIP,
                ipRangeModel.ToIP,
                ipRangeModel.CountryCode,
                ipRangeModel.CountryName,
                ipRangeModel.Region,
                ipRangeModel.City,
                ipRangeModel.ISPName));
        }

        public List<IpRangeModel> GetIpV6Range(int fromId, int toId)
        {
            return ipRangeDataAccess.GetIpV6Range(fromId, toId);
        }

        public void SaveCSV(IEnumerable<IpRangeModel> IpRanges, DateTime updatedDate, string filePath)
        {
            var ipRangeEncode = IpRanges.Select(x => {
                x.IPRangeLocationCode = EncodeIPAttribute(x);
                x.FromIP = x.FromIP.Trim();
                x.ToIP = x.ToIP.Trim();
                return x;
            }).ToList();           

            foreach (var s in ipRangeEncode)
            {
                var ipRangeResult = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                   s.IPRangeLocationCode, s.FromIP, s.ToIP, s.CountryCode, s.CountryName, s.Region, s.City, s.ISPName,0, updatedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                using (StreamWriter sw = File.AppendText(filePath))
                {                  
                    sw.WriteLine(ipRangeResult);                    
                }
            }
        }
    }
}

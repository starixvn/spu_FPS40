using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //DbSettingProviderManager
            //      .StartNewSession()                
            //      .WithMySql()
            //      .UseDefaultDbSettingProvider(resourcePath: "~/App_Data/NAPFPSNames.xml")
            //      .UseDefaultConnectionStringProvider()
            //      .Run();

            //var repository = new DynamicRepository();

            //var criteria = new LockTestCriteria();

            //Parallel.Invoke(() => {
            //    repository.Execute(criteria);
            //},
            //() => {
            //    repository.Execute(criteria);
            //});


            StreamReader sr = new StreamReader(@"D:\ipdb.csv");
  
            var ipranges = new List<IpRangeModel>();

            while (!sr.EndOfStream)
            {
               
                var strline = sr.ReadLine();
                var _values = strline.Split(',');
                Console.WriteLine(_values[0].Trim());
                if(BigInteger.Parse(_values[0].Trim()) > BigInteger.Parse("55866868554796440974603389092679909376"))
                {
                    var rangeIp = new IpRangeModel
                    {
                        FromIP = _values[0].Trim(),
                        ToIP = _values[1].Trim(),
                        CountryCode = _values[2],
                        CountryName = _values[3],
                        Region = _values[4],
                        City = _values[5],
                        ISPName = _values[6]
                    };

                    ipranges.Add(rangeIp);                   
                }
                
            }

            SaveCSV(ipranges, DateTime.Now, @"D:\ipdbUpdated129.csv");

            sr.Close();            

            Console.ReadKey();
        }

        public static void SaveCSV(IEnumerable<IpRangeModel> IpRanges, DateTime updatedDate, string filePath)
        {
            var ipRangeEncode = IpRanges.Select(x =>
            {
                x.IPRangeLocationCode = EncodeIPAttribute(x);
                x.FromIP = x.FromIP.Trim();
                x.ToIP = x.ToIP.Trim();
                return x;
            }).ToList();

            foreach (var s in ipRangeEncode)
            {
                var ipRangeResult = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
                   s.IPRangeLocationCode, s.FromIP, s.ToIP, s.CountryCode, s.CountryName, s.Region, s.City, s.ISPName, 0, updatedDate.ToString("yyyy-MM-dd HH:mm:ss"));

                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(ipRangeResult);
                }
            }
        }

        private static Int64 EncodeIPAttribute(IpRangeModel ipRangeModel)
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
    }
}

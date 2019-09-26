using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.Repository;
using Fanex.DeviceDetector.Factory;
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
    static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }     

    }

    class Program
    {
        static void Main(string[] args)
        {
            string aa = "86693a2b90f14c3ab0a8735e56f8c779";
            var d = DeviceFactory.GetDeviceInfo(aa);
            Console.WriteLine(); 

            //var items = new List<int>();

            //for (var i = 0; i < 10; i++)
            //{
            //    items.Add(i);
            //}            

            //var result = items.Partition(3);



            Console.WriteLine();


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

            //var allFiles = Directory.GetFiles(@"D:\IpDB_Data");

            //foreach (var file in allFiles)
            //{
            //    StreamReader sr = new StreamReader(file);

            //    //var ipranges = new List<IpRangeModel>();

            //    while (!sr.EndOfStream)
            //    {

            //        var strline = sr.ReadLine();
            //        var _values = strline.Split('|');
            //        Console.WriteLine(_values[0].Trim());
            //        if (BigInteger.Parse(_values[1].Trim()) == BigInteger.Parse("281473959936872"))
            //        {
            //            //var rangeIp = new IpRangeModel
            //            //{
            //            //    FromIP = _values[0].Trim(),
            //            //    ToIP = _values[1].Trim(),
            //            //    CountryCode = _values[2],
            //            //    CountryName = _values[3],
            //            //    Region = _values[4],
            //            //    City = _values[5],
            //            //    ISPName = _values[6]
            //            //};
            //    ISPName = _values[6]
            //};

            //            //ipranges.Add(rangeIp);

            //            Console.WriteLine(file);
            //        }

            //    }

            //    sr.Close();

            //}



            //  SaveCSV(ipranges, DateTime.Now, @"D:\ipdbUpdated129.csv");



        }



        //  SaveCSV(ipranges, DateTime.Now, @"D:\ipdbUpdated129.csv");



        //Console.ReadKey();
    }

    //public static void SaveCSV(IEnumerable<IpRangeModel> IpRanges, DateTime updatedDate, string filePath)
    //{
    //    var ipRangeEncode = IpRanges.Select(x =>
    //    {
    //        x.IPRangeLocationCode = EncodeIPAttribute(x);
    //        x.FromIP = x.FromIP.Trim();
    //        x.ToIP = x.ToIP.Trim();
    //        return x;
    //    }).ToList();

    //    foreach (var s in ipRangeEncode)
    //    {
    //        var ipRangeResult = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}",
    //           s.IPRangeLocationCode, s.FromIP, s.ToIP, s.CountryCode, s.CountryName, s.Region, s.City, s.ISPName, 0, updatedDate.ToString("yyyy-MM-dd HH:mm:ss"));

    //        using (StreamWriter sw = File.AppendText(filePath))
    //        {
    //            sw.WriteLine(ipRangeResult);
    //        }
    //    }
    //}

    //private static Int64 EncodeIPAttribute(IpRangeModel ipRangeModel)
    //{
    //    return Cipher.Hashing2Int(string.Format("{0}{1}{2}{3}{4}{5}{6}",
    //        ipRangeModel.FromIP,
    //        ipRangeModel.ToIP,
    //        ipRangeModel.CountryCode,
    //        ipRangeModel.CountryName,
    //        ipRangeModel.Region,
    //        ipRangeModel.City,
    //        ipRangeModel.ISPName));
    //}
    //}
}

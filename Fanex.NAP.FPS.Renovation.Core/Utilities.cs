namespace Fanex.NAP.FPS.Renovation.Core
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Linq;
    using System.IO;

    public static class Utilities
    {
        public static string GetURLFromDeviceCode(string di)
        {
            if (string.IsNullOrWhiteSpace(di)) return string.Empty;

            var requestData = di.Split(new[] { "###" }, StringSplitOptions.None);

            var url = requestData.Length >= 8 ? requestData[7] : string.Empty;

            return url;
        }

        public static string GetUserAgentFromDeviceCode(string di)
        {
            if (string.IsNullOrWhiteSpace(di)) return string.Empty;

            var deviceSplited = di.Split(new[] { "###" }, StringSplitOptions.None);

            return deviceSplited.Length > 3 ? deviceSplited[3] : string.Empty;
        }

        public static string GetDeviceCodeFromCapturedData(string di)
        {
            string[] splitingDI = di.Split(new[] { "###" }, StringSplitOptions.None);

            return splitingDI.Length >= 7 ? splitingDI[6] : string.Empty;
        }

        public static string XMLConvert(object data)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };

            var serializer = new XmlSerializer(data.GetType(), new XmlRootAttribute("root"));

            using (var stringWriter = new System.IO.StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    serializer.Serialize(writer, data, emptyNamespaces);

                    return stringWriter.ToString();
                }
            }
        }

        public static bool IsIPv4(string ip)
        {
            IPAddress address;

            if (IPAddress.TryParse(ip, out address))
            {
                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        return true;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        return false;
                    default:
                        return false;
                }
            }

            return false;
        }

        public static string Encode(string original)
        {
            if (string.IsNullOrWhiteSpace(original)) return string.Empty;

            byte[] encodedBytes;

            using (var md5 = new MD5CryptoServiceProvider())
            {
                var originalBytes = Encoding.Default.GetBytes(original);
                encodedBytes = md5.ComputeHash(originalBytes);
            }

            return Convert.ToBase64String(encodedBytes);
        }

        public static IEnumerable<IEnumerable<TValue>> Chunk<TValue>(
            this IEnumerable<TValue> values,
            Int32 chunkSize)
        {
            return values
                .Select((v, i) => new { v, groupIndex = i / chunkSize })
                .GroupBy(x => x.groupIndex)
                .Select(g => g.Select(x => x.v));
        }

        public static void WriteLog(string filename, string content)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);                      

            File.AppendAllText(filePath, string.Format("{0}{1}", content, System.Environment.NewLine));
        }

        public static void DeleteLog(string fileName)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static void WriteLogAllLine(string filename, string[] content)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);

            File.WriteAllLines(filePath, content);

        }

        public static int ConvertIpAddressToInt(string ipAddress)
        {
            var ipCode = IPAddress.Parse(ipAddress);

            var bytes = ipCode.GetAddressBytes();

            return BitConverter.ToInt32(bytes, 0);
        }

        public static string ConvertIntToIpAddress(int ipCode)
        {
            var getBytes = BitConverter.GetBytes(ipCode);
            var ipAddress = new IPAddress(getBytes);

            return ipAddress.ToString();
        }
    }
}

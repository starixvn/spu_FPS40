namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
    using Quartz;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.AccessControl;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IPRangeService ipRangeService;

        private readonly int batchSize = 10000;
 
        private readonly string fileName = "IpRange{0}.csv";

        private readonly string originalPath;

        public Job()
        {
            ipRangeService = new Services.Implement.IPRangeService();

            originalPath = AppDomain.CurrentDomain.BaseDirectory + @"\IpRangeData\";

            if(!Directory.Exists(originalPath))
            {
                Directory.CreateDirectory(originalPath);
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var ipDBVersionDate = ipRangeService.GetIPDBVersion();
                var ipLatestIPDBLogDate = ipRangeService.GetLatestUpdatedDateIPTableLog();

                if (ipDBVersionDate.CompareTo(ipLatestIPDBLogDate) != 0)
                {
                    var fileNameFormated = string.Format(fileName, batchSize);

                    var filePath = Path.Combine(originalPath, fileNameFormated);                   

                    UpdateIpTableV6(ipDBVersionDate, 0, batchSize, filePath);

                    ipRangeService.InsertNewTableLog(ipDBVersionDate);                    
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(true);
        }

        private void UpdateIpTableV6(DateTime updatedDate, int fromId, int toId, string filePath)
        {           
            var ipV6Ranges = ipRangeService.GetIpV6Range(fromId, toId);

            if (ipV6Ranges != null && ipV6Ranges.Any())
            {
                var chunkIpV6Range = ipV6Ranges.Chunk(1000);

                foreach(var ipRange in chunkIpV6Range)
                {                    
                    ipRangeService.SaveCSV(ipRange, updatedDate, filePath);
                }               

                fromId = (toId + 1);
                toId += batchSize;

                var getFileName = Path.GetFileName(filePath);

                SaveACopyfileToServer(filePath, @"\\10.18.200.72\fps40\" + $"{updatedDate.ToString("yyyyMMddHHmmss")}" + @"\" + getFileName);

                var fileNameFormated = string.Format(fileName, toId);

                var nexFilePath = Path.Combine(originalPath, fileNameFormated);

                UpdateIpTableV6(updatedDate, fromId, toId, nexFilePath);
            }
        }

        private static int ExecuteCommand(string command, int timeout)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = "C:\\",
            };

            var process = Process.Start(processInfo);
            process.WaitForExit(timeout);
            var exitCode = process.ExitCode;
            process.Close();
            return exitCode;
        }

        private static void SaveACopyfileToServer(string filePath, string savePath)
        {
            var directory = Path.GetDirectoryName(savePath).Trim();
            var username = "root";
            var password = "1234@qwer";
            var filenameToSave = Path.GetFileName(savePath);

            if (!directory.EndsWith("\\"))
                filenameToSave = "\\" + filenameToSave;

            var command = "NET USE " + directory + " /delete";
            ExecuteCommand(command, 5000);

            command = "NET USE " + directory + " /user:" + username + " " + password;
            ExecuteCommand(command, 5000);

            if(!Directory.Exists(directory))
            {
                var parentDir = Directory.GetParent(directory);

                foreach (DirectoryInfo dir in parentDir.GetDirectories())
                {
                    dir.Delete(true);
                }

                Directory.CreateDirectory(directory);
            }

            var destinationFilePath = directory + filenameToSave;

            if (File.Exists(destinationFilePath))
            {
                File.Delete(destinationFilePath);
            }

            File.Copy(filePath, destinationFilePath);

            File.Delete(filePath);
        }
    }
}

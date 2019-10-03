namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Jobs.UpdateIPRange
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Quartz;
    using Renci.SshNet;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    [DisallowConcurrentExecution]
    public class Job : IJob
    {
        private readonly Services.Interface.IPRangeService ipRangeService;        
        private readonly string rangeIpCSVFileName = "IpRanges.csv";
        private readonly string rangeIPTXTFileName = "IpRanges.txt";
        private readonly string originalPath;                       
        private readonly ConnectionInfo connectionInfo;
        private readonly string sshUser = "root";
        private readonly string sshPassword = "1234@qwer";
        private readonly string sshHost = "10.18.200.72";
        private readonly int sshPort = 22;
        private readonly string sshFolderPath = @"\\10.18.200.72\fps40\";
        private readonly string sshScript = "sh /etc/init.d/fps40_mysql_initial_ip_range.sh";
        private readonly int timeout = 5000;
        private int fromId;
        private int toId;

        public Job()
        {
            ipRangeService = new Services.Implement.IPRangeService();

            originalPath = AppDomain.CurrentDomain.BaseDirectory + @"\IpRangeData\";

            if(!Directory.Exists(originalPath))
            {
                Directory.CreateDirectory(originalPath);
            }

            var authMethod = new PasswordAuthenticationMethod(sshUser, sshPassword);

            connectionInfo = new ConnectionInfo(sshHost, sshPort, sshUser, authMethod);
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                var ipDBVersionDate = ipRangeService.GetIPDBVersion();
                var ipLatestIPDBLogDate = ipRangeService.GetLatestUpdatedDateIPTableLog();

                if (ipDBVersionDate.CompareTo(ipLatestIPDBLogDate) != 0)
                {
                    var filePath = Path.Combine(originalPath, rangeIpCSVFileName);

                    UpdateIpTableV6(ipDBVersionDate, filePath);
                }
            }
            catch (Exception ex)
            {
                var logMessage = new LogMessage(ex.Message)
                    .AddSection("Data Info", DatabaseInfoGetters.GetDbExceptionInfo(ex))
                    .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(ex)).ToString();

                LogService.Error(logMessage);
            }

            return Task.FromResult(true);
        }

        private void UpdateIpTableV6(DateTime updatedDate, string filePath)
        {
            var rangeIpUpdatedPath = Path.Combine(originalPath, rangeIPTXTFileName);

            ipRangeService.GetRangeIpId(ref fromId, ref toId, Core.AppSettings.SizeOfIpRange, rangeIpUpdatedPath);

            var ipV6Ranges = ipRangeService.GetIpV6Range(fromId, toId);

            if (ipV6Ranges != null && ipV6Ranges.Any())
            {
                ipRangeService.SaveCSV(ipV6Ranges, updatedDate, filePath);            
                       
                SaveACopyfileToServer(filePath, Path.Combine(sshFolderPath, rangeIpCSVFileName));

                using (var client = new SshClient(connectionInfo))
                {
                    client.Connect();

                    var sshCommand = client.RunCommand(sshScript);

                    if (string.IsNullOrWhiteSpace(sshCommand.Error))
                    {
                        var result = ipRangeService.UpdateIpRange(updatedDate);

                        if (result >= 0)
                        {
                            Console.WriteLine($"IP Range is updated at {DateTime.Now} - {toId}");                           

                            fromId = (toId + 1);
                            toId += Core.AppSettings.SizeOfIpRange;

                            ipRangeService.SaveRangeIpId(fromId, toId, rangeIpUpdatedPath);
                        }
                    }
                }                              
            }
            else
            {
                ipRangeService.InsertNewTableLog(updatedDate);

                fromId = toId = 0;

                ipRangeService.SaveRangeIpId(fromId, toId, rangeIpUpdatedPath);
            }
        }

        private void ExecuteCommand(string command, int timeout)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", "/C " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = "C:\\",
            };

            var process = Process.Start(processInfo);
            process.WaitForExit(timeout);
           
            process.Close();           
        }

        private void SaveACopyfileToServer(string filePath, string savePath)
        {
            var directory = Path.GetDirectoryName(savePath).Trim();

            var filenameToSave = Path.GetFileName(savePath);

            if (!directory.EndsWith("\\"))
                filenameToSave = "\\" + filenameToSave;

            var command = "NET USE " + directory + " /delete";
            ExecuteCommand(command, timeout);

            command = "NET USE " + directory + " /user:" + sshUser + " " + sshPassword;
            ExecuteCommand(command, timeout);

            if(!Directory.Exists(directory))
            {
                var parentDir = Directory.GetParent(directory);

                foreach (DirectoryInfo dir in parentDir.GetDirectories())
                {
                    dir.Delete(true);
                }

                Directory.CreateDirectory(directory);
            }                       

            var destinationDirInfo = new DirectoryInfo(directory);

            foreach (FileInfo file in destinationDirInfo.GetFiles())
            {
                file.Delete();
            }

            var destinationFilePath = directory + filenameToSave;

            File.Copy(filePath, destinationFilePath);

            File.Delete(filePath);
        }
    }
}

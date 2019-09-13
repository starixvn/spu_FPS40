namespace Fanex.NAP.FPS.Renovation.DataTransform.Service
{
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
    using Microsoft.Owin.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.ServiceProcess;
    using System.Threading.Tasks;
    using System.Timers;

    public partial class Runner : ServiceBase
    {
        private IDisposable _webApp;

        public Runner()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _webApp = WebApp.Start<Startup>(AppSettings.SelfHostUrl);
            }
            catch
            {
                var netsh = Path.Combine(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.System), "netsh.exe");
                                
                var startInfo = new ProcessStartInfo(netsh);
                startInfo.Arguments = "http add urlacl url="+ AppSettings.SelfHostUrl + " user=Everyone listen=yes";
                startInfo.UseShellExecute = true;
                startInfo.Verb = "runas";
                var process = Process.Start(startInfo);
                process.WaitForExit();

                _webApp = WebApp.Start<Startup>(AppSettings.SelfHostUrl);
            }
        }

        protected override void OnStop()
        {
            if (_webApp != null)
            {
                _webApp.Dispose();
            }

            LogService.Info($"Data Transform service is STOPED at {DateTime.Now}");
        }
    }
}

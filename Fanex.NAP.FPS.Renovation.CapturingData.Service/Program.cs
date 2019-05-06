namespace Fanex.NAP.FPS.Renovation.CapturingData.Service
{
    using Fanex.Data;
    using Fanex.Data.MySql;
    using Fanex.NAP.FPS.Renovation.Core;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.ServiceProcess;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            DbSettingProviderManager
             .StartNewSession()
             .WithMySql(resourcePath: AppSetting.AppDataSetting) 
             .Run();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Runner()
            };

            if (Environment.UserInteractive)
            {
                // run console
                var type = typeof(ServiceBase);
                const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                var method = type.GetMethod("OnStart", Flags);

                foreach (var service in ServicesToRun)
                {
                    method.Invoke(service, new object[] { args });
                }

                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();

                foreach (var service in ServicesToRun)
                {
                    service.Stop();
                }
            }
            else
            {
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

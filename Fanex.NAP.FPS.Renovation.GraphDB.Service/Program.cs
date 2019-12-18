using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Graph.Data;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Xml.Linq;

namespace Fanex.NAP.FPS.Renovation.GraphDB.Service
{
    static class Program
    {
        private static IDictionary<string, GraphSetting> settings = new Dictionary<string, GraphSetting>();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            RegisterXmlFile("~/App_Data");

            DbSessionFactory.Init()
                .RegisterConfigs("DB.MiddleService")
                .RegisterXmlFile("~/App_Data")
                .RegisterDriverFactory(new Neo4jDriverFactory())
                .RegisterCommandBuilder(new Neo4jCommandBuilder("~/App_Data/scripts"))
                .RegisterCommandExecutor(new Neo4jExecutor())
                .Run();

            LogManager
                .SetDefaultLogCategory(Core.AppSettings.FanexLogCategory)
                .Use(new List<ILogEngine>
                {
                    new SentryLogging(new SentryEngineOptions
                    {
                        Dsn = new Dsn(Core.AppSettings.SentryLogging)
                    })
                });

            DbSettingProviderManager
                .StartNewSession()                
                .WithMySql()
                .UseDefaultDbSettingProvider(resourcePath: Core.AppSettings.AppDataSetting)
                .UseDefaultConnectionStringProvider()
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

        private static void RegisterXmlFile(string dir)
        {
            var files = Directory.GetFiles(ResolvePath(dir), "*.xml", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                var document = XDocument.Load(file);

                foreach (var setting in document.Descendants("GraphSetting"))
                {
                    var key = setting.Attribute("Key").Value;
                    var spName = setting.Attribute("SpName").Value;
                    var db = setting.Attribute("DbName").Value;

                    if (key is null)
                    {
                        throw new InvalidOperationException($"There's no Key attribute for {setting}");
                    }

                    if (spName is null)
                    {
                        throw new InvalidOperationException($"There's no SpName attribute for {setting}");
                    }

                    if (db is null)
                    {
                        throw new InvalidOperationException($"There's no DbName attribute for {setting}");
                    }

                    settings.Add(setting.Attribute("Key").Value, new GraphSetting
                    {
                        DbKey = db,
                        SpKey = key,
                        SpName = spName
                    });
                }
            }
        }

        private static string ResolvePath(string path)
        {
            DirectoryInfo result;
            if (path.StartsWith("~"))
            {
                var location = Assembly.GetExecutingAssembly().CodeBase;
                var file = new FileInfo(location.Replace("file:///", ""));
                var isConsole = file.ToString().Contains("exe");

                if (isConsole)
                {
                    var arr = path.Replace("~/", "").Split('/');
                    result = file.Directory;

                    for (int i = 0; i < arr.Length - 1; i++)
                    {
                        result = result.GetDirectories(arr[i], SearchOption.AllDirectories)[0];
                    }

                    return $"{result.FullName}\\{arr[arr.Length - 1]}";
                }
                else
                {
                    result = file.Directory.Parent.GetDirectories(path.Replace("~/", ""))[0];
                    return result.FullName;
                }

            }

            return path;
        }
    }
}

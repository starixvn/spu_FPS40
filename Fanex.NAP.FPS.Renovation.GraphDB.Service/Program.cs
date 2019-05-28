using Fanex.Graph.Data;
using Fanex.NAP.FPS.Renovation.GraphDB.Service.Neo4j;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.GraphDB.Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            DbSessionFactory.Init()
                .RegisterConfigs("DB.MiddleService")
                .RegisterXmlFile("~/App_Data")
                .RegisterDriverFactory(new Neo4jDriverFactory())
                .RegisterCommandBuilder(new Neo4jCommandBuilder("~/App_Data/scripts"))
                .RegisterCommandExecutor(new Neo4jExecutor())
                .Run();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Runner()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}

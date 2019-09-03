using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DbSettingProviderManager
                  .StartNewSession()                
                  .WithMySql()
                  .UseDefaultDbSettingProvider(resourcePath: "~/App_Data/NAPFPSNames.xml")
                  .UseDefaultConnectionStringProvider()
                  .Run();

            var repository = new DynamicRepository();

            var criteria = new LockTestCriteria();

            Parallel.Invoke(() => {
                repository.Execute(criteria);
            },
            () => {
                repository.Execute(criteria);
            });


            Console.ReadKey();
        }
    }
}

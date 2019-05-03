namespace Fanex.NAP.FPS.Renovation.Core
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class AppSetting
    {
        public static int Environment
        {
            get
            {

                var environment = ConfigurationManager.AppSettings["Environment"];

                switch (environment)
                {
                    case "1":
                        return Convert.ToInt16(Model.Environment.Local);
                    case "2":
                        return Convert.ToInt16(Model.Environment.UAT);
                    default:
                        return Convert.ToInt16(Model.Environment.Production);
                }
            }
        }

        public static string Host => ConfigurationManager.AppSettings["Queue-Host"];

        public static string Post => ConfigurationManager.AppSettings["Queue-Port"];

        public static string VirtualHost => ConfigurationManager.AppSettings["Queue-VirtualHost"];

        public static string UserName => ConfigurationManager.AppSettings["Queue-UserName"];

        public static string Password => ConfigurationManager.AppSettings["Queue-Password"];

        public static int PrefetchCount => Convert.ToInt16(ConfigurationManager.AppSettings["Queue-PrefetchCount"]);

        public static string Exchange => ConfigurationManager.AppSettings["Queue-Exchange"];

    }
}

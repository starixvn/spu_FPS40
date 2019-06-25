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

            var encodeString = Utilities.Encode("Mozilla/5.0 (iPhone; CPU iPhone OS 12_3_1 like Mac OS X; zh-CN) AppleWebKit/537.51.1 (KHTML, like Gecko) Mobile/16F203 UCBrowser/12.4.7.1178 Mobile  AliApp(TUnionSDK/0.1.20.3)");

            Console.WriteLine(encodeString);

            Console.ReadKey();
        }
    }
}

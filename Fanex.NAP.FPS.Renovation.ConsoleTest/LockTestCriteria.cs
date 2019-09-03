using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.ConsoleTest
{
    public class LockTestCriteria : NonQueryCommand
    {
        public override string GetSettingKey()
        {
            return "LockTest";
        }

        public override bool IsValid()
        {
            return true ;
        }
    }
}

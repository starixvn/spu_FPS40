using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service
{
    public interface ISchedule
    {
        ISchedule Create();

        ISchedule Shutdown();

        ISchedule Start();

        ISchedule Pause();
    }
}

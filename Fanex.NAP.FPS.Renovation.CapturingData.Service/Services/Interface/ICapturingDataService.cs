namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    interface ICapturingDataService
    {
        IEnumerable<T> PrCondition<T>();

        void Rule();
    }
}

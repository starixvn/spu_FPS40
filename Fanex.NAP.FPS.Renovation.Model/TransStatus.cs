using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Model
{
    public enum TransStatus
    {
        /// <summary>
        /// Binary Values is 0
        /// </summary>
        ValidTransaction = -1,
        /// <summary>
        /// Binary Values is 1
        /// </summary>
        InvalidSubscriber = 15,
        /// <summary>
        /// Binary Values is 10
        /// </summary>
        InvalidActionResult = 14,
        /// <summary>
        /// Binary Values is 100
        /// </summary>
        InvalidDeviceCode = 13,
        /// <summary>
        /// Binary Values is 1000
        /// </summary>
        InvalidLoginName = 12
    }
}

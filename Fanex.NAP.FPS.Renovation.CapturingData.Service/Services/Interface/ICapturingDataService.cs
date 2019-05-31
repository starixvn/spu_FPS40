namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.NAP.FPS.Renovation.Model;
    using System.Collections.Generic;

    interface ICapturingDataService
    {     
        void InsertTransaction(List<RawTransaction> rawTrans);

        DeviceInfo GetDeviceInfo(string deviceCode);
    }
}

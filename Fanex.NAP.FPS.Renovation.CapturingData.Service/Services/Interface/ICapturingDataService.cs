namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.Oneyed.Infrastructure.Models;

    interface ICapturingDataService
    {     
        void InsertTransaction(MessageQueue message);
    }
}

namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.Services
{
    using Fanex.Oneyed.Infrastructure.Models;

    interface ICapturingDataService
    {     
        void InsertTransaction(MessageQueue message);
    }
}

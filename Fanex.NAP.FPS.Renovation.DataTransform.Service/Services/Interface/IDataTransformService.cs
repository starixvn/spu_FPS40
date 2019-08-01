using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface
{
    interface IDataTransformService
    {
        StagingRawTransactionModel GetStagingRawTransaction(int isProcessed, int numberOfTicket);

        void InsertAccounts(int fromId , int toId);
    }
}

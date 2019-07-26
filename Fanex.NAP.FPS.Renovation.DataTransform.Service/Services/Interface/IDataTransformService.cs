using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface
{
    interface IDataTransformService
    {
        List<StagingRawTransactionModel> GetStagingRawTransaction(int isProcessed, int numberOfTicket);

        void InsertTransaction(List<StagingRawTransactionModel> model);

        List<AccountExistedModel> GetAccountExist(List<TransactionModel> model);
    }
}

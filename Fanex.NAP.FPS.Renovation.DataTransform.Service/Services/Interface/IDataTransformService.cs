using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface
{
    interface IDataTransformService
    {
        List<StagingRawTransactionModel> GetStagingRawTransaction(int numberOfTicket);

        DataResult InsertTransaction(List<StagingRawTransactionModel> model);
    }
}

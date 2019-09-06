using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface
{
    interface IDataTransformService
    {
        StagingRawTransactionModel GetStagingRawTransaction(int isProcessed, int numberOfTicket);

        void AccountProcessing(int fromId, int toId);

        void DeviceProcessing(int fromId, int toId);

        void UserAgentProcessing(int fromId, int toId);

        void TransactionProcessing(int fromId, int toId);

        void UnhandleSubscriberAccountProcessing(int fromId, int toId);

        void CompleteProcessing(int fromId, int toId);
    }
}

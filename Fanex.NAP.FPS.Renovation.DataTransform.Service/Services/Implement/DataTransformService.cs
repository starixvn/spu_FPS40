using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UAParser;
using StagingRawTransactionModel = Fanex.NAP.FPS.Renovation.Model.StagingRawTransactionModel;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement
{
    class DataTransformService : IDataTransformService
    {
        private readonly DataAccess.StagingRawTransactionDataAccess stagingRawTransaction;
        private readonly DataAccess.DataCenterDataAccess dataCenter;
        public DataTransformService()
        {
            stagingRawTransaction = new DataAccess.StagingRawTransactionDataAccess();

            dataCenter = new DataCenterDataAccess();
        }

        public List<StagingRawTransactionModel> GetStagingRawTransaction(int isProcessed, int numberOfTicket)
        {
            return stagingRawTransaction.GetStagingRawTrans(isProcessed, numberOfTicket);
        }

        public DataResult InsertTransaction(List<StagingRawTransactionModel> model)
        {
            var transformData = model.Select(x => TransformTranction(x)).ToList();

            if (transformData.Any())
            {
                var xmlTrans = Utilities.XMLConvert(transformData);

                return dataCenter.InsertTransaction(xmlTrans);

            }

            return new DataResult()
            {
                ErrorCode = -1,
                ErrorMessage = "Cannot insert transaction."
            };
        }

        private TransactionModel TransformTranction(StagingRawTransactionModel model)
        {
            var uaParser = Parser.GetDefault();

            var clientInfo = uaParser.Parse(model.UserAgent);

            return new TransactionModel()
            {
                DeviceCode = model.DeviceCode,
                LoginName = model.LoginName,
                SubscriberName = model.SubscriberName,
                TransDate = model.TransDate,
                CreatedDate = model.CreatedDate,
                IP = IPAddress.Parse(model.IP).GetHashCode(),
                PluginID = model.PluginID,
                DeviceStatus = model.DeviceStatus,
                URL = model.URL,
                ActionResult = model.ActionResult,
                UserAgent = model.UserAgent,
                UserAgentCode = Utilities.Encode(model.UserAgent),
                BrowserName = clientInfo.UA.Family,
                OSName = clientInfo.OS.Family,
                Flagged = 0,
                StagingTransID = model.StagingTransId
            };
        }
    }
}

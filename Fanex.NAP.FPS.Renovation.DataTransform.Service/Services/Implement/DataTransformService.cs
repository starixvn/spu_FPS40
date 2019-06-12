using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using StagingRawTransactionModel = Fanex.NAP.FPS.Renovation.Model.StagingRawTransactionModel;
using Fanex.NAP.FPS.Renovation.Model;
using System.Net;
using BrowserNET;

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

        public List<StagingRawTransactionModel> GetStagingRawTransaction()
        {
            return stagingRawTransaction.GetStagingRawTrans();
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
            var browser = new UserAgentParser(model.UserAgent);

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
                BrowserName = browser.BrowserName,
                OSName = browser.Platform,
                Flagged = 0,
                StagingTransID = model.StagingTransId
            };
        }
    }
}

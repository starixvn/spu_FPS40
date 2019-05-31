using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using StagingRawTransaction = Fanex.NAP.FPS.Renovation.Model.StagingRawTransaction;
using Fanex.NAP.FPS.Renovation.Model;
using System.Net;
using BrowserNET;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement
{
    class DataTransformService : IDataTransformService
    {
        private readonly DataAccess.StagingRawTransaction stagingRawTransaction;
        private readonly DataAccess.DataCenter dataCenter;
        public DataTransformService()
        {
            stagingRawTransaction = new DataAccess.StagingRawTransaction();

            dataCenter = new DataCenter();
        }

        public List<StagingRawTransaction> GetStagingRawTransaction()
        {
            return stagingRawTransaction.GetStagingRawTrans();
        }

        public void InsertTransaction(List<StagingRawTransaction> model)
        {
            var transformData = model.Select(x => TransformTranction(x)).ToList();

            if(transformData.Any())
            { 
                var maxStagingTransId = transformData.Max(x => x.StagingTransID);

                var xmlTrans = Utilities.XMLConvert(transformData);

                dataCenter.InsertTransaction(xmlTrans, maxStagingTransId);
            }
        }

        private Transaction TransformTranction(StagingRawTransaction model)
        {
            var browser = new UserAgentParser(model.UserAgent);

            return new Transaction()
            {
                DeviceCode = model.DeviceCode,
                LoginName = model.LoginName,
                SubscriberName = model.SubscriberName,
                TransDate = model.TransDate,
                CreatedDate = model.CreatedDate,
                IPID = IPAddress.Parse(model.IP).GetHashCode(),
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

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataAccess;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
    using Fanex.NAP.FPS.Renovation.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UAParser;

    class DataTransformService : IDataTransformService
    {
        private readonly DataAccess.StagingRawTransactionDataAccess stagingRawTransaction;
        private readonly DataAccess.DataCenterDataAccess dataCenter;
        public DataTransformService()
        {
            stagingRawTransaction = new DataAccess.StagingRawTransactionDataAccess();

            dataCenter = new DataCenterDataAccess();
        }    

        public StagingRawTransactionModel GetStagingRawTransaction(int isProcessed, int numberOfTicket)
        {
            return stagingRawTransaction.GetStagingRawTrans(isProcessed, numberOfTicket);
        }

        public void InsertAccounts(int fromId, int toId)
        {
            var existingAccounts = AccountProcessing(fromId, toId);

            if (existingAccounts.Any())
            {
                var customerInfo = UpdateCustomerInfo(existingAccounts);

                if(customerInfo != null && customerInfo.Any())
                {
                    InsertRawAccount(customerInfo);                    
                }               
            }
        }

        private List<CustomerInfoModel> UpdateCustomerInfo(List<AccountExistedModel> accountList)
        {
            var getNewCustomer = accountList.Select(acc => TransformCustomerInfo(acc)).ToList();

            if (getNewCustomer.Any())
            {
                var accountListXml = Utilities.XMLConvert(getNewCustomer);

                return dataCenter.GetCustomerInfo(accountListXml);
            }

            return new List<CustomerInfoModel>();
        }

        private NewCustomerModel TransformCustomerInfo(AccountExistedModel model)
        {
            return new NewCustomerModel
            {
                AccountID = model.AccountID,
                LoginName = model.LoginName,
                SubscriberID = model.SubscriberID,
                SubscriberName = model.SubscriberName,
                LoginNameWithPrefix = model.LoginNameWithPrefix
            };
        }

        //private TransactionModel TransformTransaction(StagingRawTransactionModel model)
        //{
        //    var uaParser = Parser.GetDefault();

        //    var clientInfo = uaParser.Parse(model.UserAgent);

        //    var ipInfo = model.IP.Ip2Long();

        //    var ipNumber = (decimal)ipInfo.IpNumber;

        //    return new TransactionModel
        //    {
        //        StagingTransID = model.StagingTransId,
        //        DeviceCode = model.DeviceCode,
        //        FingeringCode = model.FingeringCode,
        //        FingeringMoreInfo = model.FingeringMoreInfo,
        //        LoginName = model.LoginName,
        //        SubscriberName = model.SubscriberName,
        //        TransDate = model.TransDate,
        //        CreatedDate = model.CreatedDate,
        //        IP = ipInfo.IsIpV6 ? ipNumber : ipNumber + 281470681743360,
        //        PluginID = model.PluginID,
        //        URL = model.URL,
        //        ActionResult = model.ActionResult,
        //        UserAgent = model.UserAgent,
        //        UserAgentCode = Cipher.Hashing(model.UserAgent),
        //        BrowserName = clientInfo.UA.Family,
        //        OSName = clientInfo.OS.Family,
        //        Flagged = 0,
        //        CustID = 0,
        //        AccountID = 0,
        //        CurrencyID = 0,
        //        CurrencyName = string.Empty,
        //        RoleID = 0,
        //        Username = string.Empty,
        //        Username2 = String.Empty
        //    };
        //}

        private List<AccountExistedModel> AccountProcessing(int fromId, int toId)
        {
            return dataCenter.AccountProcess(fromId, toId);
        }

        private void InsertRawAccount(List<CustomerInfoModel> accounts)
        {
            var accountListXml = Utilities.XMLConvert(accounts);

            dataCenter.InsertRawAccount(accountListXml);
        }

        public void UserAgentProcessing(int fromId, int toId)
        {
            dataCenter.UserAgentProcess(fromId, toId);
        }

        public void TransactionProcessing(int fromId, int toId)
        {
            dataCenter.TransactionProcess(fromId, toId);
        }
    }
}

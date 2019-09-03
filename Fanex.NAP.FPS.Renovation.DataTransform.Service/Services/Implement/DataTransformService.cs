namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Implement
{
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.DataAccess;
    using Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface;
    using Fanex.NAP.FPS.Renovation.Model;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

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
            System.Console.WriteLine("1.Get Raw Transaction");

            return stagingRawTransaction.GetStagingRawTrans(isProcessed, numberOfTicket);
        }

        public void AccountProcessing(int fromId, int toId)
        {
            System.Console.WriteLine("3.Account Process");

            var existingAccounts = AccountProcess(fromId, toId);            

            if (existingAccounts.Any())
            {
                var customerInfo = UpdateCustomerInfo(existingAccounts);               

                if (customerInfo != null && customerInfo.Any())
                {
                    InsertRawAccount(customerInfo);                    
                }               
            }
        }

        private List<CustomerInfoModel> UpdateCustomerInfo(List<AccountExistedModel> accountList)
        {
            System.Console.WriteLine("=> 3.2.Update Customer Info");

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

        private List<AccountExistedModel> AccountProcess(int fromId, int toId)
        {
            System.Console.WriteLine("=> 3.1.Get Existing Account");

            return dataCenter.AccountProcess(fromId, toId);
        }

        private void InsertRawAccount(List<CustomerInfoModel> accounts)
        {
            System.Console.WriteLine("=> 3.3.Insert Raw Account");

            var accountListJson = JsonConvert.SerializeObject(accounts);

            dataCenter.InsertRawAccount(accountListJson);
        }

        public void UserAgentProcessing(int fromId, int toId)
        {
            dataCenter.UserAgentProcess(fromId, toId);

            System.Console.WriteLine("4.UserAgent Process");
        }

        public void TransactionProcessing(int fromId, int toId)
        {
            dataCenter.TransactionProcess(fromId, toId);

            System.Console.WriteLine("6.Transaction Process");
        }

        public void UnhandleSubscriberAccountProcessing(int fromId, int toId)
        {
            System.Console.WriteLine("2.UnhandleSubscriberActionProcess");

            dataCenter.UnhandleSubscriberActionProcess(fromId, toId);
        }

        public void DeviceProcessing(int fromId, int toId)
        {
            dataCenter.DeviceProcess(fromId, toId);

            System.Console.WriteLine("5.Device Process");
        }

        public void CompleteProcessing(int fromId, int toId)
        {
            dataCenter.CompleteProcess(fromId, toId);

            System.Console.WriteLine("7.Completed Process");
        }

        public void UpdateTransactionDevice(int fromId, int toId)
        {
            dataCenter.UpdateTransactionDevice(fromId, toId);

            System.Console.WriteLine("8.Update Transaction Device");
        }

        public void UpdateTransactionAccount(int fromId, int toId)
        {
            dataCenter.UpdateTransactionAccount(fromId, toId);

            System.Console.WriteLine("9.Update Transaction Account");
        }
    }
}

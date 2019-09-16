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
        private readonly DataAccess.DataCenterDataAccess dataCenter;
        public DataTransformService()
        {
            dataCenter = new DataCenterDataAccess();
        }  

        public void AccountProcessing(int fromId, int toId)
        {
            var existingAccounts = AccountProcess(fromId, toId);

            if (existingAccounts.Any())
            {
                var customerInfo = UpdateCustomerInfo(existingAccounts);
                                     
                if (customerInfo != null && customerInfo.Any())
                {
                    var newCustomerInfo = customerInfo.Select(x =>
                    {
                        x.Site = x.Site ?? string.Empty;
                        x.Username = x.Username ?? string.Empty;
                        x.Username2 = x.Username2 ?? string.Empty;
                        x.CurrencyName = x.CurrencyName ?? string.Empty;

                        return x;
                    }).ToList();

                    var customerInfoMapping = existingAccounts
                        .Where(x => !newCustomerInfo.Any(a => a.LoginName == x.LoginName || a.LoginNameWithPrefix == x.LoginNameWithPrefix))
                        .Select(s => new CustomerInfoModel
                        {
                            AccountID = s.AccountID,
                            CreatedDate = s.CreatedDate,
                            CurrencyID = 0,
                            CurrencyName = string.Empty,
                            CustID = 0,
                            IsDeposit = s.IsDeposit,
                            LoginName = s.LoginName,
                            LoginNameWithPrefix = s.LoginNameWithPrefix,
                            RoleID = 0,
                            Site = string.Empty,
                            SubAccId = 0,
                            SubscriberID = s.SubscriberID,
                            SubscriberName = s.SubscriberName,
                            Username = string.Empty,
                            Username2 = string.Empty
                        })
                        .ToList();

                    var latestCustomerInfo = newCustomerInfo.Union(customerInfoMapping).ToList();

                    InsertRawAccount(latestCustomerInfo);
                }
            }

            System.Console.WriteLine("3.Account Process");
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
            var accountListJson = JsonConvert.SerializeObject(accounts);

            dataCenter.InsertRawAccount(accountListJson);

            System.Console.WriteLine("=> 3.3.Insert Raw Account");
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
            dataCenter.UnhandleSubscriberActionProcess(fromId, toId);

            System.Console.WriteLine("2.UnhandleSubscriberActionProcess");
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
    }
}

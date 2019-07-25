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

        public List<AccountExistingModel> GetAccountExist(List<TransactionModel> model)
        {
            var accountExistList = model.Select(x => TransformAccountExist(x)).ToList();

            if (accountExistList.Any())
            {
                var xmlAccount = Utilities.XMLConvert(accountExistList);

                return stagingRawTransaction.GetAccountExist(xmlAccount);
            }

            return new List<AccountExistingModel>();
        }

        public List<StagingRawTransactionModel> GetStagingRawTransaction(int isProcessed, int numberOfTicket)
        {
            return stagingRawTransaction.GetStagingRawTrans(isProcessed, numberOfTicket);
        }

        public void InsertTransaction(List<StagingRawTransactionModel> model)
        {
            var transformData = model.Select(x => TransformTransaction(x)).ToList();

            if (transformData.Any())
            {
                var existingAccounts = GetAccountExist(transformData);

                if (existingAccounts.Any())
                {
                    var existedAccounts = existingAccounts.GroupBy(p => p.IsExisted, (key, group) => new
                    {
                        Existed = key,
                        AccountList = group.ToList()
                    });                                        

                    var resultCustomerInfo = new List<TransactionModel>();

                    var resultAccountExist = new List<TransactionModel>();

                    _ = Parallel.ForEach(existedAccounts, x =>
                      {
                         var transactions = transformData
                          .Select(newTrans=> {
                              var account = x.AccountList.FirstOrDefault(acc => 
                              CompareStringData(acc.LoginName, newTrans.LoginName) &&
                              CompareStringData(acc.SubscriberName, newTrans.SubscriberName));

                              newTrans.IsDeposit = account != null ? account.IsDeposit : 0;
                              newTrans.SubscriberID = account != null ? account.SubscriberID : 0;

                              return newTrans;
                          })
                          .Where(trans => x.AccountList.Any(acc =>
                              CompareStringData(acc.LoginName, trans.LoginName) &&
                              CompareStringData(acc.SubscriberName, trans.SubscriberName)))
                          .ToList();

                          if (x.Existed)
                          {
                              resultAccountExist = transactions;
                          }
                          else
                          {
                              resultCustomerInfo = UpdateCustomerInfo(transactions, x.AccountList);
                          }
                      });

                   var resultTransaction = resultCustomerInfo
                        .Union(resultAccountExist)
                        .OrderBy(x => x.StagingTransID)
                        .ToList();

                    AddTransaction(resultTransaction);
                }
            }
        }

        //private List<TransactionModel> UpdateTransaction(List<TransactionModel> transactions, List<AccountExistingModel> accountList)
        //{
        //    if (transactions.Any() && accountList.Any())
        //    {
        //        var result = transactions.Select(trans =>
        //        {
        //            var customerInfo = accountList.FirstOrDefault(c =>
        //                CompareStringData(c.SubscriberName, trans.SubscriberName) &&
        //                CompareStringData(c.LoginName, trans.LoginName));

        //            trans.SubscriberID = customerInfo.SubscriberID;                   

        //            return trans;
        //        });

        //        return result.ToList();
        //    }

        //    return new List<TransactionModel>();
        //}

        private List<TransactionModel> UpdateCustomerInfo(List<TransactionModel> transactions, List<AccountExistingModel> accountList)
        {
            if (transactions.Any() && accountList.Any())
            {
                var getNewCustomer = accountList.Select(acc => TransformCustomerInfo(acc)).ToList();

                if (getNewCustomer.Any())
                {
                    var accountListXml = Utilities.XMLConvert(getNewCustomer);

                    var getCustomerInfo = dataCenter.GetCustomerInfo(accountListXml);

                    if (getCustomerInfo.Any() && transactions.Any())
                    {
                        var result = transactions.Select(trans =>
                          {
                              var customerInfo = getCustomerInfo.FirstOrDefault(c =>
                                  CompareStringData(c.SubscriberName, trans.SubscriberName) &&
                                  CompareStringData(c.LoginName, trans.LoginName));

                              if (customerInfo != null)
                              {
                                  trans.AccountID = customerInfo.AccountID;
                                  trans.CustID = customerInfo.CustID;
                                  trans.CurrencyID = customerInfo.CurrencyID;
                                  trans.CurrencyName = customerInfo.CurrencyName;
                                  trans.Username = customerInfo.Username;
                                  trans.Username2 = customerInfo.Username2;
                                  trans.RoleID = customerInfo.RoleID;                                  
                              }

                              return trans;

                          });

                        return result.ToList();
                    }

                    return transactions;
                }
            }

            return new List<TransactionModel>();
        }

        private void AddTransaction(List<TransactionModel> transactions)
        {
            if (transactions.Any())
            {
                var xmlTrans = Utilities.XMLConvert(transactions);

                dataCenter.InsertTransaction(xmlTrans);
            }
        }

        private NewCustomerModel TransformCustomerInfo(AccountExistingModel model)
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


        private AccountExistingModel TransformAccountExist(TransactionModel model)
        {
            return new AccountExistingModel
            {
                LoginName = model.LoginName,
                SubscriberName = model.SubscriberName,
                IsExisted = false,
                SubscriberID = 0,
                AccountID = 0
            };
        }

        private TransactionModel TransformTransaction(StagingRawTransactionModel model)
        {
            var uaParser = Parser.GetDefault();

            var clientInfo = uaParser.Parse(model.UserAgent);

            var ipInfo = model.IP.Ip2Long();

            var ipNumber = (decimal)ipInfo.IpNumber;

            return new TransactionModel
            {
                StagingTransID = model.StagingTransId,
                DeviceCode = model.DeviceCode,
                FingeringCode = model.FingeringCode,
                FingeringMoreInfo = model.FingeringMoreInfo,
                LoginName = model.LoginName,
                SubscriberName = model.SubscriberName,
                TransDate = model.TransDate,
                CreatedDate = model.CreatedDate,
                IP = ipInfo.IsIpV6 ? ipNumber : ipNumber + 281470681743360,
                PluginID = model.PluginID,
                URL = model.URL,
                ActionResult = model.ActionResult,
                UserAgent = model.UserAgent,
                UserAgentCode = Cipher.Hashing(model.UserAgent),
                BrowserName = clientInfo.UA.Family,
                OSName = clientInfo.OS.Family,
                Flagged = 0,
                CustID = 0,
                AccountID = 0,
                CurrencyID = 0,
                CurrencyName = string.Empty,
                RoleID = 0,
                Username = string.Empty,
                Username2 = String.Empty
            };
        }

        private static Boolean CompareStringData(string a, string b) => string.Compare(a, b, true) == 0;
    }
}

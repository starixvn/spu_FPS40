using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement
{
    public class DataCenterService : IDataCenterService
    {
        private readonly DataAccess.DataCenterDataAccess dataCenter;
        
        public DataCenterService()
        {
            dataCenter = new DataCenterDataAccess();
        }

        public List<AssociationModel> GetAssociationForGrapDB(int isSync, int size)
        {
            return dataCenter.GetAssociation(isSync, size);
        }

        public List<CustomerInfoModel> GetCustomerInfo(List<NewCustomerModel> newCustomers)
        {
            if (newCustomers != null && newCustomers.Any())
            {
                var newXmlCustomer = Utilities.XMLConvert(newCustomers);

                return dataCenter.GetCustomerInfo(newXmlCustomer);
            }

            return new List<CustomerInfoModel>();
        }       

        public List<NewCustomerModel> GetNewCustomer(int size)
        {
            return dataCenter.GetNewCustomer(size);
        }

        public List<int> GetUpdatedAccount(int size)
        {
            return dataCenter.GetUpdatedAccountList(size);
        }

        public List<DevicePackage> GetStagingTransIdForDevice(int size)
        {
            return dataCenter.GetStagingTransIdForDevice(size);
        }

        public DataResult UpdateAccountId(string accountIdJson)
        {
            return dataCenter.UpdateAccountId(accountIdJson);
        }

        public DataResult UpdateCustomerInfo(List<CustomerInfoModel> customerInfo)
        {
            var newCustomerInfo = customerInfo.Select(x => TransformCustomerInfo(x)).ToList();

            var newJsonCustomerInfo = JsonConvert.SerializeObject(newCustomerInfo);

            return dataCenter.UpdateCustomerInfo(newJsonCustomerInfo);
        }

        public DataResult InsertDevice(List<DevicePackage> transId)
        {
            var transJson = JsonConvert.SerializeObject(transId);

            return dataCenter.InsertDevice(transJson);
        }

        public DataResult UpdateIsSyncByAssociationId(string associationXml)
        {
            return dataCenter.UpdateIsSyncGraphDBByAssociationId(associationXml);
        }

        private NewCustomerInfo TransformCustomerInfo(CustomerInfoModel customerInfo)
        {
            return new NewCustomerInfo
            {
                CurrencyID = customerInfo.CurrencyId,
                CurrencyName = customerInfo.CurrencyName,
                CustID = customerInfo.SubAccId != 0 ? customerInfo.SubAccId : customerInfo.CustID,
                IsDeposit = customerInfo.IsDeposit,
                RoleID = customerInfo.RoleID,
                Username = customerInfo.Username,
                Username2 = customerInfo.Username2,
                CreatedDate = customerInfo.CreatedDate,
                AccountID = customerInfo.AccountId,
                SubscriberID = customerInfo.SubscriberId
            };
        }
    }
}

using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
{
    public class DataCenterService : IDataCenterService
    {
        private readonly DataAccess.DataCenter dataCenter;
        
        public DataCenterService()
        {
            dataCenter = new DataAccess.DataCenter();
        }

        public List<CustomerInfo> GetCustomerInfo(List<NewCustomer> newCustomers)
        {
            if (newCustomers != null && newCustomers.Any())
            {
                var newXmlCustomer = Utilities.XMLConvert(newCustomers);

                return dataCenter.GetCustomerInfo(newXmlCustomer);
            }

            return new List<CustomerInfo>();
        }

        public List<NewCustomer> GetNewCustomer()
        {
            return dataCenter.GetNewCustomer();
        }

        public DataResult UpdateCustomerInfo(List<CustomerInfo> customerInfo)
        {
            var newCustomerInfo = customerInfo.Select(x => TransformCustomerInfo(x));

            var newXmlCustomerInfo = Utilities.XMLConvert(newCustomerInfo);

            return dataCenter.UpdateCustomerInfo(newXmlCustomerInfo);
        }

        private NewCustomerInfo TransformCustomerInfo(CustomerInfo customerInfo)
        {
            return new NewCustomerInfo()
            {
                CurrencyID = customerInfo.CurrencyId,
                CurrencyName = customerInfo.CurrencyName,
                CustID = customerInfo.SubAccId != 0 ? customerInfo.SubAccId : customerInfo.CustId,
                IsDeposit = customerInfo.IsDeposit,
                RoleID = customerInfo.RoleId,
                Username = customerInfo.Username,
                Username2 = customerInfo.Username2,
                CreatedDate = customerInfo.CreatedDate,
                AccountID = customerInfo.AccountId,
                SubscriberID = customerInfo.SubscriberId
            };
        }
    }
}

using Fanex.NAP.FPS.Renovation.Core;
using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
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

        public List<IpRangeModel> GetIpV4Range() => dataCenter.GetIpV4Range().Select(selector: x => {
            x.IPType = 0;
            return x;
        }).ToList();

        public List<IpRangeModel> GetIpV6Range() => dataCenter.GetIpV6Range().Select(x => {
            x.IPType = 1;
            return x;
        }).ToList();

        public List<NewCustomerModel> GetNewCustomer(int size)
        {
            return dataCenter.GetNewCustomer(size);
        }

        public NewIPTableUpdatedModel GetNewIPTableUpdatedLog(int ipType)
        {
            return dataCenter.GetNewIPTableUpdatedLog(ipType);
        }

        public DataResult UpdateCustomerInfo(List<CustomerInfoModel> customerInfo)
        {
            var newCustomerInfo = customerInfo.Select(x => TransformCustomerInfo(x)).ToList();

            var newXmlCustomerInfo = Utilities.XMLConvert(newCustomerInfo);

            return dataCenter.UpdateCustomerInfo(newXmlCustomerInfo);
        }

        public DataResult UpdateIpRange(List<IpRangeModel> IpRanges)
        {
            var ipRangeXml = Utilities.XMLConvert(IpRanges);

            return dataCenter.UpdateIPRange(ipRangeXml: ipRangeXml);
        }

        public DataResult UpdateIpTableLog(int ipType)
        {
            return dataCenter.UpdateIpTableLog(ipType);
        }

        private NewCustomerInfo TransformCustomerInfo(CustomerInfoModel customerInfo)
        {
            return new NewCustomerInfo
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

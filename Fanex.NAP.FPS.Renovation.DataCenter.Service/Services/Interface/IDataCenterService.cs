using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Org.BouncyCastle.Asn1;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
{
    interface IDataCenterService
    {
        List<NewCustomerModel> GetNewCustomer(int size);

        DataResult UpdateCustomerInfo(List<CustomerInfoModel> customerInfo);

        List<CustomerInfoModel> GetCustomerInfo(List<NewCustomerModel> newCustomers);

        List<AssociationModel> GetAssociationForGrapDB(int isSync, int size);

        DataResult UpdateIpRange(List<IpRangeModel> IpRanges);

        List<IpRangeModel> GetIpV4Range();

        List<IpRangeModel> GetIpV6Range();

        NewIPTableUpdatedModel GetNewIPTableUpdatedLog(int ipType);

        DataResult UpdateIpTableLog(int ipType);
    }
}

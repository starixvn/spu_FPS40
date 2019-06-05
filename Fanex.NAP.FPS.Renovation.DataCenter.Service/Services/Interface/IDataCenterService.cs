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
        List<NewCustomer> GetNewCustomer();

        DataResult UpdateCustomerInfo(List<CustomerInfo> customerInfo);

        List<CustomerInfo> GetCustomerInfo(List<NewCustomer> newCustomers);
    }
}

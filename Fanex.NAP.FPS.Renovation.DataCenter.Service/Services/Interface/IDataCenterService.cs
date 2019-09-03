using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
{
    interface IDataCenterService
    {
        List<NewCustomerModel> GetNewCustomer(int size);

        DataResult UpdateCustomerInfo(List<CustomerInfoModel> customerInfo);

        List<CustomerInfoModel> GetCustomerInfo(List<NewCustomerModel> newCustomers);

        List<AssociationModel> GetAssociationForGrapDB(int isSync, int size);

        DataResult UpdateIsSyncByAssociationId(string associationXml);
    }
}

using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Interface
{
    public interface IFraudSensorService
    {
        List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(ICriteria criteria);

        List<AssociationByCustomerModel> GetAssociationByCustomers(ICriteria criteria);

        List<CustomerEvidence> GetCustomersEvidences(ICriteria criteria);
    }
}

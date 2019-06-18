using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Model;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface
{
    public interface IFraudSensorService
    {
        List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(string custIds);

        List<AssociationByCustomerModel> GetAssociationByCustomers(string custIds);

        List<CustomerEvidence> GetCustomersEvidences(ICriteria criteria);
    }
}

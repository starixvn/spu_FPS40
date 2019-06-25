using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using System.Collections.Generic;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement
{
    public class FraudSensorService : IFraudSensorService
    {
        private readonly AssociationDataAccess associationDataAccess;

        public FraudSensorService()
        {
            associationDataAccess = new AssociationDataAccess();
        }

        public List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(ICriteria criteria)
        {
            return associationDataAccess.GetData<TotalAssociationByCustomerModel>(criteria);
        }

        public List<AssociationByCustomerModel> GetAssociationByCustomers(ICriteria criteria)
        {
            return associationDataAccess.GetData<AssociationByCustomerModel>(criteria);
        }             

        public List<CustomerEvidence> GetCustomersEvidences(ICriteria criteria)
        {
            return associationDataAccess.GetData<CustomerEvidence>(criteria);
        }
    }
}

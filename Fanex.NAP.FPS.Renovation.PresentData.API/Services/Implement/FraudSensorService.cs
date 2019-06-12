using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement
{
    public class FraudSensorService : IFraudSensorService
    {
        private readonly AssociationDataAccess associationDataAccess;
        public FraudSensorService()
        {
            associationDataAccess = new AssociationDataAccess();
        }

        public List<AssociationByCustomerModel> GetAssociationByCustomers(string custIds)
        {
            return associationDataAccess.GetAssociationByCustomer(custIds);
        }

        public List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(string custIds)
        {
            return associationDataAccess.GetTotalAssociationByCustomer(custIds);
        }
    }
}

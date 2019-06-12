using Fanex.NAP.FPS.Renovation.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface
{
    public interface IFraudSensorService
    {
        List<TotalAssociationByCustomerModel> GetTotalAssociationByCustomer(string custIds);

        List<AssociationByCustomerModel> GetAssociationByCustomers(string custIds);
    }
}

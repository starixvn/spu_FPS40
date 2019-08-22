using Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Implement;
using Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Controllers
{
    public class FraudSensorController : ApiController
    {
        private readonly IFraudSensorService fraudSensorService;

        public FraudSensorController()
        {
            fraudSensorService = new FraudSensorService();
        }

        //[HttpPost]
        //public JsonResult GetTotalAssociationByCustomer([FromBody]GetTotalAssociationByCustomerCriteria criteria)
        //{
        //    var data = fraudSensorService.GetTotalAssociationByCustomer(criteria);

        //    return new JsonResult(data);
        //}

        //[HttpPost]
        //public JsonResult GetAssociationByCustomer([FromBody]GetAssociationByCustomerCriteria criteria)
        //{
        //    var data = fraudSensorService.GetAssociationByCustomers(criteria);

        //    return new JsonResult(data);
        //}

        //[HttpPost]
        //public JsonResult GetEvidencesByCustomers([FromBody]GetCustomersEvidencesCriteria criteria)
        //{
        //    var data = fraudSensorService.GetCustomersEvidences(criteria);

        //    return new JsonResult(data);
        //}

        //[HttpGet]
        //public JsonResult Testing()
        //{
        //    return new JsonResult("testing");
        //}
    }
}

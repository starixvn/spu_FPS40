using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudSensorController : ControllerBase
    {
        private readonly IFraudSensorService fraudSensorService;

        public FraudSensorController()
        {
            fraudSensorService = new FraudSensorService();
        }

        [HttpPost]
        public JsonResult GetTotalAssociationByCustomer([FromBody] string custIds)
        {
            var data = fraudSensorService.GetTotalAssociationByCustomer(custIds);

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult GetAssociationByCustomer([FromBody] string custIds)
        {
            var data = fraudSensorService.GetAssociationByCustomers(custIds);

            return new JsonResult(data);
        }

       // [Authorize]
       [HttpGet]
        public JsonResult Testing()
        {
            return new JsonResult("testing");
        }
    }
}
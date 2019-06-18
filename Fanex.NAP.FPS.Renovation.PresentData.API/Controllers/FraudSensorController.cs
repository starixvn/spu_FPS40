using Fanex.NAP.FPS.Renovation.Criteria;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{   
    [Route("api/[controller]/[action]")]
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

        [HttpPost]      
        public JsonResult GetEvidencesByCustomers(GetCustomersEvidencesCriteria criteria)
        {
            var data = fraudSensorService.GetCustomersEvidences(criteria);

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
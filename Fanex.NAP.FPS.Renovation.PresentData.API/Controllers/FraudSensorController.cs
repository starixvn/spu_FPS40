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
        public JsonResult GetTotalAssociationByCustomer([FromBody]GetTotalAssociationByCustomerCriteria criteria)
        {
            var data = fraudSensorService.GetTotalAssociationByCustomer(criteria);

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult GetAssociationByCustomer([FromBody]GetAssociationByCustomerCriteria criteria)
        {
            var data = fraudSensorService.GetAssociationByCustomers(criteria);

            return new JsonResult(data);
        }

        [HttpPost]      
        public JsonResult GetEvidencesByCustomers([FromBody]GetCustomersEvidencesCriteria criteria)
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
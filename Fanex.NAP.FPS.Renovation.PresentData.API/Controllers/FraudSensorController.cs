using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FraudSensorController : ControllerBase
    {
        [HttpGet("{customers}")]
        public JsonResult GetTotalAssociationByCustomer(string customers)
        {
            var data = new List<TotalAssociationByCustomer>();

            data.Add(new TotalAssociationByCustomer {
                CustId = 1,
                TotalAssocaition = 2
            });

            data.Add(new TotalAssociationByCustomer
            {
                CustId = 3,
                TotalAssocaition = 4
            });

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult GetAssociationByCustomer([FromBody] string custIds)
        {
            return null;
        }

        [HttpPost]
        public JsonResult GetEvidenceByCustomer()
        {
            return null;
        }
    }
}
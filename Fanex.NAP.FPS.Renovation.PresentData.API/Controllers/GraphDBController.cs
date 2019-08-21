using Fanex.Graph.Data;
using Fanex.NAP.FPS.Renovation.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GraphDBController : Controller
    {
        public JsonResult Testing()
        {
            return new JsonResult("testing");
        }

        public List<Customer> GetAssociationOneLevel(int custid)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationOneLevelByCustId", new { CustId = custid }).Data.ToList();
        }

        public List<Customer> GetAssociationNLevel(int custid)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationNLevelByCustId", new { CustId = custid }).Data.ToList();
        }
    }
}
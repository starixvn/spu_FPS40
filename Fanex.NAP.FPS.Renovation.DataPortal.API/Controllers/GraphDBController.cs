using Fanex.Graph.Data;
using Fanex.NAP.FPS.Renovation.DataPortal.API.Models;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Controllers
{

    public class GraphDBController : ApiController
    {
        [HttpPost]
        public List<Customer> GetAssociationOneLevel([FromBody]int custId)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationOneLevelByCustId", new { CustId = custId }).Data.ToList();
        }

        [HttpPost]
        public List<Customer> GetAssociationNLevel([FromBody]int custId)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationNLevelByCustId", new { CustId = custId }).Data.ToList();
        }

        [HttpPost]
        public JsonResult<ShortestPathOfAccount> GetShortestPath([FromBody]int startCustId, [FromBody]int endCustId)
        {
            var path = DbSessionFactory.ExecuteSingle<ShortestPathOfAccount>("GetShortestPath", new { StartCustId = startCustId, EndCustId = endCustId }).Data;
            return Json(path);
        }
    }
}

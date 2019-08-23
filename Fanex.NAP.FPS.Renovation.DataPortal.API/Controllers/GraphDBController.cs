using Fanex.Graph.Data;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Controllers
{
    public class GraphDBController : ApiController
    {
        [HttpPost]
        public List<Customer> GetAssociationOneLevel([FromBody]int custid)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationOneLevelByCustId", new { CustId = custid }).Data.ToList();
            
        }

        [HttpPost]
        public List<Customer> GetAssociationNLevel([FromBody]int custid)
        {
            return DbSessionFactory.Execute<Customer>("GetAssociationNLevelByCustId", new { CustId = custid }).Data.ToList();
        }

    }
}

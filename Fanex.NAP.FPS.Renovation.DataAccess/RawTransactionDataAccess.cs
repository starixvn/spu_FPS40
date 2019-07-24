using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.NAP.FPS.Renovation.Criteria;
using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    public class RawTransactionDataAccess
    {
        public int InsertTransaction(string xmlTrans)
        {
            var repository = new DynamicRepository();

            var criteria = new CapturingTransactionCriteria
            {
                RawTransXml = xmlTrans   
            };

           return repository.Execute(criteria);
        }  
    }
}

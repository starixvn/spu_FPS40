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
    public class RawTransaction
    {
        public void InsertTransaction(string xmlTrans)
        {
            var repository = new DynamicRepository();

            var criteria = new CapturingTransactionCriteria()
            {
                RawTransXml = xmlTrans   
            };

            repository.Execute(criteria);
        }
    }
}

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
    public class DataCenter
    {
        public DataResult InsertTransaction(string xmlTrans, int maxStagingTransId)
        {
            var repository = new DynamicRepository();
            
            var criteria = new InsertDataCenterTransactionCriteria()
            {
                StagingTransXml = xmlTrans,
                MaxStagingTransId = maxStagingTransId,
                ErrCode = 0,
                ErrMessage = String.Empty
            };

            repository.Execute(criteria);

            return new DataResult()
            {
                ErrorCode = criteria.ErrCode,
                ErrorMessage = criteria.ErrMessage
            };
        }
    }
}

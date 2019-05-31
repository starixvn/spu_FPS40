using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;

namespace Fanex.NAP.FPS.Renovation.DataTransform.Service.Services.Interface
{
    interface IDataTransformService
    {
        List<StagingRawTransaction> GetStagingRawTransaction();

        void InsertTransaction(List<StagingRawTransaction> model);
    }
}

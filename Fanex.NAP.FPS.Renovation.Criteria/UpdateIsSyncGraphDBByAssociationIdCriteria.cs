using Fanex.Data;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria
{
    public class UpdateIsSyncGraphDBByAssociationIdCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_AssociationXML")]
        public string AssociationXml { get; set; }

        public override string GetSettingKey()
        {
            return "DC_UpdateAssociation";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

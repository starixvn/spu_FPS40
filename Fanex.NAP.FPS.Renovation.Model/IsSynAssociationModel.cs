using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "association")]
    public class IsSynAssociationModel
    {
        [XmlAttribute]
        public long AssociationID { get; set; }

        [XmlAttribute]
        public int SyncGraphDB { get; set; }
    }
}

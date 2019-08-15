using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "os")]
    public class OSModel
    {
        [XmlIgnore]
        public int OSId { get; set; }

        [XmlAttribute(AttributeName = "OSName")]
        public string OSName { get; set; }

        [XmlAttribute(AttributeName = "UserAgentID")]
        public int UserAgentId { get; set; }

        [XmlIgnore]
        public int IsExisting { get; set; }
    }
}

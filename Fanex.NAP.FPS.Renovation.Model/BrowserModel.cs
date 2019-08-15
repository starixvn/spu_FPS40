using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "browser")]
    public class BrowserModel
    {
        [XmlIgnore]
        public int BrowserId { get; set; }


        [XmlAttribute(AttributeName = "Browser")]
        public string BrowserName { get; set; }


        [XmlAttribute(AttributeName = "UserAgentID")]
        public int UserAgentId { get; set; }

        [XmlIgnore]
        public int IsExisting { get; set; }
    }
}

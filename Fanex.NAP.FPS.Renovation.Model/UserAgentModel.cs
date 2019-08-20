using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "useragent")]
    public class UserAgentModel
    {
        [XmlAttribute(AttributeName = "UserAgentID")]
        public int UserAgentID { get; set; }

        [XmlIgnore]
        public string UserAgent { get; set; }

        [XmlAttribute(AttributeName = "OSID")]
        public int OSId { get; set; }

        [XmlAttribute(AttributeName = "BrowserID")]
        public int BrowserId { get; set; }

        [XmlAttribute(AttributeName = "OSName")]
        public string OSName { get; set; }

        [XmlAttribute(AttributeName = "BrowserName")]
        public string BrowserName { get; set; }
    }
}

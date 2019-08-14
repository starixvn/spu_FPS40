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

        [XmlAttribute(AttributeName = "Browser")]
        public string BrowserName { get; set; }

        [XmlAttribute(AttributeName = "OSName")]
        public string OSName { get; set; }

    }
}

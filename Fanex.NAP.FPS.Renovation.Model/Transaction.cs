using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "transaction")]
    public class Transaction
    {
        [XmlAttribute]
        public int StagingTransID { get; set; }

        [XmlAttribute]
        public string DeviceCode { get; set; }

        [XmlAttribute]
        public string LoginName { get; set; }

        [XmlAttribute]
        public string SubscriberName { get; set; }

        [XmlAttribute]
        public DateTime TransDate { get; set; }

        [XmlAttribute]
        public DateTime CreatedDate { get; set; }

        [XmlAttribute]
        public int IPID { get; set; }

        [XmlAttribute]
        public int PluginID { get; set; }

        [XmlAttribute]
        public int DeviceStatus { get; set; }

        [XmlAttribute]
        public string URL { get; set; }

        [XmlAttribute]
        public string ActionResult { get; set; }

        [XmlAttribute]
        public string UserAgent { get; set; }

        [XmlAttribute]
        public string UserAgentCode { get; set; }

        [XmlAttribute]
        public string BrowserName { get; set; }

        [XmlAttribute]
        public string OSName { get; set; }

        [XmlAttribute]
        public int Flagged { get; set; }
    }
}

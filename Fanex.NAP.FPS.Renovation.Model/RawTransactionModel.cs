using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "transaction")]
    public class RawTransactionModel
    {
        [XmlAttribute]
        public string DeviceCode { get; set; }

        [XmlAttribute]
        public string LoginName { get; set; }

        [XmlAttribute]
        public string SubscriberName { get; set; }

        [XmlAttribute]
        public DateTime TransDate { get; set; }

        [XmlAttribute]
        public string IP { get; set; }

        [XmlAttribute]
        public int PluginID { get; set; }

        [XmlAttribute]
        public int DeviceCodeStatus { get; set; }

        [XmlAttribute]
        public string URL { get; set; }

        [XmlAttribute]
        public string ActionResult { get; set; }

        [XmlAttribute]
        public string UserAgent { get; set; }

        [XmlAttribute]
        public string FingeringCode { get; set; }

        [XmlAttribute]
        public string FingeringMoreInfo { get; set; }
    }
}

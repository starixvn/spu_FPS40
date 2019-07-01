using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "ip")]
    public class IpRangeModel
    {
        [XmlAttribute]
        public string IpRangeCode { get; set; }

        [XmlAttribute]
        public int IPType { get; set; }

        [XmlAttribute]
        public BigInteger FromIp { get; set; }

        [XmlAttribute]
        public BigInteger ToIp { get; set; }

        [XmlAttribute]
        public string CountryCode { get; set; }

        [XmlAttribute]
        public string CountryName { get; set; }

        [XmlAttribute]
        public string Region { get; set; }

        [XmlAttribute]
        public string ISPName { get; set; }
    }
}

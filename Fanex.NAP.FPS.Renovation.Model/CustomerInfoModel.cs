using System;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    public class CustomerInfoModel
    {
        [XmlAttribute]
        public int AccountID { get; set; }

        [XmlAttribute]
        public int SubscriberID { get; set; }

        [XmlAttribute]
        public int CustID { get; set; }
                
        public int SubAccId { get; set; }

        [XmlAttribute]
        public string Username { get; set; }

        [XmlAttribute]
        public string Username2 { get; set; }

        [XmlAttribute]
        public int RoleID { get; set; }

        public string Site { get; set; }

        [XmlAttribute]
        public int IsDeposit { get; set; }

        [XmlAttribute]
        public int CurrencyID { get; set; }

        [XmlAttribute]
        public string CurrencyName { get; set; }

        [XmlAttribute]
        public DateTime CreatedDate { get; set; }

        [XmlAttribute]
        public string LoginName { get; set; }

        [XmlAttribute]
        public string SubscriberName { get; set; }
    }
}

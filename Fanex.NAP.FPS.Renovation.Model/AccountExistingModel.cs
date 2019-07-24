using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fanex.NAP.FPS.Renovation.Model
{
    [XmlType(TypeName = "account")]
    public class AccountExistingModel
    {
        [XmlAttribute(AttributeName = "LoginName")]
        public string LoginName { get; set; }

        public string LoginNameWithPrefix { get; set; }

        public int SubscriberID { get; set; }

        [XmlAttribute(AttributeName = "SubName")]
        public string SubscriberName { get; set; }

        public int AccountID { get; set; }

        public bool IsExisted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Model
{
    public class RawTransactionModel
    {
        public string Account { get; set; }

        public string Action { get; set; }

        public string ActionResult { get; set; }

        public string Device { get; set; }

        public string City { get; set; }
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public string Flag { get; set; }

        public string IP { get; set; }

        public string ISP { get; set; }

        public string Region { get; set; }

        public string Subscriber { get; set; }

        public DateTime SubscriberDate { get; set; }

        public string Token { get; set; }

        public string CaptchaResult { get; set; }
    }
}

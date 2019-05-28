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

        public string Ip { get; set; }

        public string Subscriber { get; set; }

        public DateTime SubscriberDate { get; set; }

        public string Token { get; set; }

        public string CaptchaResult { get; set; }
    }
}

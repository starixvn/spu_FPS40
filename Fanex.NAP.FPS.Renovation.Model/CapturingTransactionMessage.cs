namespace Fanex.NAP.FPS.Renovation.Model
{
     using System;

    [Serializable]
    public class CapturingTransactionMessage
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

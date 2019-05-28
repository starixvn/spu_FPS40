namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data.Repository;
    using System;

    public class CapturingTransactionCriteria : NonQueryCommand
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

        public override string GetSettingKey() => "FPS_Capturing_Transaction_Insert";

        public override bool IsValid() => true;
    }
}

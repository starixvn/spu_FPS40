namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using System;

    public class CapturingTransactionCriteria
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

        //public override string GetSettingKey() => "FS_Rpt_OfficeIP_Get";

        //public override bool IsValid() => true;
    }
}

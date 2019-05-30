namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data.Repository;
    using System;

    public class CapturingTransactionCriteria : NonQueryCommand
    {
        public string inRawTransXml { get; set; }

        public override string GetSettingKey() => "RT_RawTrans_InsertByXML";

        public override bool IsValid() => true;
    }
}

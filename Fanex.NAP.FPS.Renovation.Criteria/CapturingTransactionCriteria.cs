namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class CapturingTransactionCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_RawTransXml")]
        public string RawTransXml { get; set; }

        public override string GetSettingKey() => "RT_RawTrans_InsertByXML";

        public override bool IsValid() => true;
    }
}

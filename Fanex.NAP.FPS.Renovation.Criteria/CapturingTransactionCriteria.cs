namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class CapturingTransactionCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_RawTransJson")]
        public string RawTransXml { get; set; }

        public override string GetSettingKey() => "FPS_RT_RawTrans_InsertByXML";

        public override bool IsValid() => true;
    }
}

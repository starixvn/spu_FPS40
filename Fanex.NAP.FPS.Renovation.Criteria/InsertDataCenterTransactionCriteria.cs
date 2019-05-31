namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using System;

    public class InsertDataCenterTransactionCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "ip_TransXml")]
        public string StagingTransXml { get; set; }

        [SpParam(SqlParamName = "op_ErrorCode")]
        public int ErrCode { get; set; }

        [SpParam(SqlParamName = "op_ErrorMessage")]
        public string ErrMessage { get; set; }

        [SpParam(SqlParamName = "MaxStagingTransID")]
        public int MaxStagingTransId { get; set; }

        public override string GetSettingKey() => "DC_DataCenter_ProcessTransaction";

        public override bool IsValid() => true;
    }
}

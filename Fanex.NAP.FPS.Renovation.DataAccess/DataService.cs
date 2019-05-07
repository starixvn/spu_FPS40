namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.FPS.Renovation.DataAccess.Resources;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;

    public static class DataService
    {
        public static IEnumerable<T> GetData<T>(CriteriaBase criteria)
        {
            using (IObjectDb objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return objectDb.Query<T>(criteria);
            }
        }

        public static DBResult UpdateData(CriteriaBase criteria, string prefix)
        {
            using (var objectDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                objectDb.ExecuteNonQuery(param);

                var code = param["ErrCode"];
                var intCode = (code is DBNull) ? 0 : Convert.ToInt32(code);

                return new DBResult
                {
                    ErrorCode = intCode,
                    ErrorMsg = ErrorLookup.GetValue(prefix + "_" + intCode),
                    OutParam = param
                };
            }
        }
    }
}

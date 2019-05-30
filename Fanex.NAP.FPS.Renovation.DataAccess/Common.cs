using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Criteria;
using Fanex.Data.Repository;
using Fanex.Data;

namespace Fanex.NAP.FPS.Renovation.DataAccess
{
    public class Common
    {
        public IpInfo GetIpInfo(string ipAddress)
        {
            var criteria = new IpInfoCriteria()
            {
                IpAddress = ipAddress
            };

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.QueryEntity<IpInfo>(criteria);
            }
        }
    }
}

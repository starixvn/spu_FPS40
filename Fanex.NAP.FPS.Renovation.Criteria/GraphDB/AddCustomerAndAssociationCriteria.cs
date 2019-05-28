using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.Criteria.GraphDB
{
    public class AddCustomerAndAssociationCriteria
    {
        private string username2;
        private int? roleId;

        public int DeviceId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Username2
        {
            get => username2 == string.Empty ? null : username2;
            set { username2 = value; }
        }

        public int? RoleId
        {
            get => roleId == 0 ? null : roleId;

            set { roleId = value; }
        }

        public string CreatedDate { get; set; }
    }
}

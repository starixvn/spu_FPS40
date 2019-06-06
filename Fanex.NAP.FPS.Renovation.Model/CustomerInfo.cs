using System;

namespace Fanex.NAP.FPS.Renovation.Model
{
    public class CustomerInfo
    {
        public int AccountId { get; set; }

        public int SubscriberId { get; set; }

        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public int RoleId { get; set; }

        public string Site { get; set; }

        public int IsDeposit { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

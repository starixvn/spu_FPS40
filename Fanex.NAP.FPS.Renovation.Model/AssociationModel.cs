namespace Fanex.NAP.FPS.Renovation.Model
{
    public class AssociationModel
    {
        public int AssociationID { get; set; }

        public int DeviceID { get; set; }

        public int SubscriberID { get; set; }

        public string SubscriberName { get; set; }

        public int IsDeposit { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public string Username2 { get; set; }

        public int RoleID { get; set; }

        public int CurrencyID { get; set; }

        public string CurrencyName { get; set; }
    }
}

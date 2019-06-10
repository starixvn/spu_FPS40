namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data.Repository;
    public class GetNewCustomerCriteria : CriteriaBase
    {
        public int Size { get; set; }

        public override string GetSettingKey()
        {
            return "DC_GetNewCustomer";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}

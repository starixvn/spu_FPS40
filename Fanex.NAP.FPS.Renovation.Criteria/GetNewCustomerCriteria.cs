namespace Fanex.NAP.FPS.Renovation.Criteria
{
    using Fanex.Data.Repository;
    public class GetNewCustomerCriteria : CriteriaBase
    {
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

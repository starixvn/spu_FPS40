namespace Fanex.NAP.FPS.Renovation.CapturingData.Service.Services
{
    using Fanex.NAP.FPS.Renovation.Criteria;
    using Fanex.NAP.FPS.Renovation.DataAccess;
    using Fanex.NAP.FPS.Renovation.Model;

    public class CapturingDataService : ICapturingDataService
    {
        private CapturingTransactionMessage _message = null;

        public CapturingDataService(CapturingTransactionMessage message)
        {
            _message = message;
        }

        public void InsertCapturingTransaction()
        {
            var criteria = new CapturingTransactionCriteria()
            {
                Account = _message.Account,
                Action = _message.Action,
                ActionResult = _message.ActionResult,
                Device = _message.Device,
                Ip = _message.Ip,
                Subscriber = _message.Subscriber,
                SubscriberDate = _message.SubscriberDate,
                Token = _message.Token,
                CaptchaResult = _message.CaptchaResult
            };

            DataService.UpdateData(criteria, string.Empty);
        }

    }
}

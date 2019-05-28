namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.NAP.FPS.Renovation.Model;
    using Fanex.Oneyed.Infrastructure.Models;

    public class CapturingDataService : ICapturingDataService
    {
        private DataAccess.CapturingData capturingData;
        public CapturingDataService()
        {
            capturingData = new DataAccess.CapturingData();
        }

        public void InsertTransaction(MessageQueue message)
        {
            var model = new RawTransactionModel()
            {
                Account = message.Account,
                Action = message.Action,
                ActionResult = message.ActionResult,
                Device = message.Device,
                Ip = message.IP,
                Subscriber = message.Subscriber,
                SubscriberDate = message.SubscriberDate,
                Token = message.Token,
                CaptchaResult = string.Empty
            };

            capturingData.InsertTransaction(model);
        }

    }
}

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
                Subscriber = message.Subscriber,
                SubscriberDate = message.SubscriberDate,
                Token = message.Token,
                CaptchaResult = string.Empty,
                City = message.IpInfo.City,
                CountryCode = message.IpInfo.CountryCode,
                CountryName = message.IpInfo.CountryName,
                Flag = message.IpInfo.Flag,
                IP = message.IpInfo.IP,
                ISP = message.IpInfo.ISP,
                Region = message.IpInfo.Region
            };

            capturingData.InsertTransaction(model);
        }
    }
}

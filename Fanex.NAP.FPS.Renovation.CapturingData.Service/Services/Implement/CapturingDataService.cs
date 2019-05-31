namespace Fanex.NAP.FPS.Renovation.RawTransaction.Service
{
    using Fanex.DeviceDetector.Factory;
    using Fanex.NAP.FPS.Renovation.Core;
    using Fanex.NAP.FPS.Renovation.Model;
    using System.Collections.Generic;

    public class CapturingDataService : ICapturingDataService
    {
        private readonly DataAccess.CapturingData capturingData;
        private readonly DataAccess.Common common;
        private const string _EMPTY = "-";
        public CapturingDataService()
        {
            capturingData = new DataAccess.CapturingData();
            common = new DataAccess.Common();
        }

        public void InsertTransaction(List<RawTransaction> rawTrans)
        {
            var model = Utilities.XMLConvert(rawTrans);

            capturingData.InsertTransaction(model);
        }

        public DeviceInfo GetDeviceInfo(string deviceCode)
        {
            if (string.IsNullOrWhiteSpace(deviceCode))
            {
                return TransformEmptyDeviceInfo();
            }

            var deviceInfo = DeviceFactory.GetDeviceInfo(deviceCode);

            if (deviceInfo != null)
            {
                return TransformDeviceInfo(deviceInfo);
            }

            return TransformEmptyDeviceInfo();
        }

        private DeviceInfo TransformEmptyDeviceInfo()
        {
            return new DeviceInfo
            {
                AppVersion = "N/A",
                CapturedDate = 0,
                DeviceFPCode = "N/A",
                DeviceNo = "N/A",
                HRef = "N/A",
                NA = "N/A",
                Screen = new ScreenInfo
                {
                    ColorDepth = 0,
                    Height = 0,
                    Width = 0
                },
                TimeZone = "N/A",
                UserAgent = "N/A"
            };
        }

        private DeviceInfo TransformDeviceInfo(DeviceDetector.Models.DeviceInfo deviceInfo)
        {
            return new DeviceInfo
            {
                AppVersion = deviceInfo.AppVersion,
                CapturedDate = deviceInfo.CapturedDate,
                DeviceFPCode = deviceInfo.DeviceCode,
                DeviceNo = deviceInfo.DeviceId,
                HRef = deviceInfo.HRef,
                NA = deviceInfo.NA,
                TimeZone = deviceInfo.TimeZone,
                UserAgent = deviceInfo.UserAgent,
                Screen = deviceInfo.Screen == null
                    ? new ScreenInfo
                    {
                        ColorDepth = 0,
                        Height = 0,
                        Width = 0
                    }
                    : new ScreenInfo
                    {
                        ColorDepth = deviceInfo.Screen.ColorDepth,
                        Height = deviceInfo.Screen.Height,
                        Width = deviceInfo.Screen.Width
                    }
            };
        }
    }
}

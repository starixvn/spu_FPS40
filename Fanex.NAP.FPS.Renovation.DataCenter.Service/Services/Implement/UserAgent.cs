using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UAParser;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Implement
{
    public class UserAgent : IUserAgent
    {
        private readonly DataAccess.DataCenterDataAccess dataCenter;

        public UserAgent()
        {
            dataCenter = new DataCenterDataAccess();
        }

        public List<BrowserModel> GetBrowser(List<BrowserModel> browserList)
        {
            var browserJson = JsonConvert.SerializeObject(browserList);

            return dataCenter.GetExistingBrowser(browserJson);
        }

        public List<OSModel> GetOS(List<OSModel> osList)
        {
            var osJson = JsonConvert.SerializeObject(osList);

            return dataCenter.GetExistingOS(osJson);
        }

        public List<UserAgentModel> GetUserAgent(int size)
        {
            var userAgents = dataCenter.GetUserAgent(size);

            var uaParser = Parser.GetDefault();

            var browserList = new List<BrowserModel>();

            var osList = new List<OSModel>();

            foreach (var userAgent in userAgents)
            {
                var clientInfo = uaParser.Parse(userAgent.UserAgent);
                var browserName = clientInfo.UA.Family;
                var osName = clientInfo.OS.Family;

                browserList.Add(new BrowserModel
                {
                    BrowserName = browserName,
                    IsExisting = 0,                    
                    UserAgentCode = userAgent.UserAgentCode
                });

                osList.Add(new OSModel
                {
                    OSName = osName,
                    IsExisting = 0,                   
                    UserAgentCode = userAgent.UserAgentCode
                });

            }

            var browserListUpdated = BrowserProcessing(browserList);

            var osListUpdated = OSProcessing(osList);


            return userAgents.Select(x => {

                var browserUpdated = browserListUpdated.FirstOrDefault(br => br.UserAgentCode == x.UserAgentCode);
                var osUpdated = osListUpdated.FirstOrDefault(os => os.UserAgentCode == x.UserAgentCode);

                x.OSId = osUpdated.OSId;
                x.BrowserId = browserUpdated.BrowserId;
                x.OSName = osUpdated.OSName;
                x.BrowserName = browserUpdated.BrowserName;

                return x;

            }).ToList();            
        }

        public int InsertBrowser(string browserName)
        {
            return dataCenter.InsertBrowser(browserName);
        }

        public int InsertOS(string osName)
        {
            return dataCenter.InsertOS(osName);
        }

        public int UpdateUserAgent(string userAgentXML)
        {
            return dataCenter.UpdateUserAgent(userAgentXML).ErrorCode;
        }

        private List<BrowserModel> BrowserProcessing(List<BrowserModel> browserList)
        {
            var browserListUpdated = GetBrowser(browserList);

            return browserListUpdated.Select(x =>
            {
                if (x.IsExisting == 0)
                {
                    x.BrowserId = InsertBrowser(x.BrowserName);
                }

                return x;
            }).ToList();
        }

        private List<OSModel> OSProcessing(List<OSModel> osList)
        {
            var osListUpdated = GetOS(osList);

            return osListUpdated.Select(x =>
            {
                if (x.IsExisting == 0)
                {
                    x.OSId = InsertOS(x.OSName);
                }

                return x;
            }).ToList();
        }

    }
}

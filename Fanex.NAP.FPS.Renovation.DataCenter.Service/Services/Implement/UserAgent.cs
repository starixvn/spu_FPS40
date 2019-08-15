using Fanex.NAP.FPS.Renovation.DataAccess;
using Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface;
using Fanex.NAP.FPS.Renovation.Model;
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
            var browserXML = Core.Utilities.XMLConvert(browserList);

            return dataCenter.GetExistingBrowser(browserXML);
        }

        public List<OSModel> GetOS(List<OSModel> osList)
        {
            var osXML = Core.Utilities.XMLConvert(osList);

            return dataCenter.GetExistingOS(osXML);
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
                    UserAgentId = userAgent.UserAgentID
                });

                osList.Add(new OSModel
                {
                    OSName = osName,
                    IsExisting = 0,
                    UserAgentId = userAgent.UserAgentID
                });

            }

            var browserListUpdated = BrowserProcessing(browserList);

            var osListUpdated = OSProcessing(osList);


            return userAgents.Select(x => {

                x.OSId = osListUpdated.FirstOrDefault(os => os.UserAgentId == x.UserAgentID).OSId;
                x.BrowserId = browserListUpdated.FirstOrDefault(br => br.UserAgentId == x.UserAgentID).BrowserId;

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

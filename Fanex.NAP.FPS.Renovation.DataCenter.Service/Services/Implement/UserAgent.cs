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

        public List<UserAgentModel> GetUserAgent(int size)
        {
            var userAgents = dataCenter.GetUserAgent(size);
            var uaParser = Parser.GetDefault();

            return userAgents.Select(x =>
            {
                var clientInfo = uaParser.Parse(x.UserAgent);

                x.BrowserName = clientInfo.UA.Family;
                x.OSName = clientInfo.OS.Family;

                return x;
            }).ToList();
        }
    }
}

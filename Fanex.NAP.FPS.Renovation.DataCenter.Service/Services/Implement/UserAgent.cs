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

        public List<UserAgentModel> GetUserAgent(int size)
        {
            var userAgents = dataCenter.GetUserAgent(size);

            var uaParser = Parser.GetDefault();           
            
            return userAgents.Select(x => {

                 var clientInfo = uaParser.Parse(x.UserAgent);
                var browserName = clientInfo.UA.Family;
                var osName = clientInfo.OS.Family;
                                
                x.OSName = osName;
                x.BrowserName = browserName;
                

                return x;

            }).ToList();            
        }

        public int UpdateUserAgent(string userAgentJson)
        {
            return dataCenter.UpdateUserAgent(userAgentJson).ErrorCode;
        }
    }
}

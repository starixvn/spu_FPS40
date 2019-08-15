using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataCenter.Service.Services.Interface
{
    interface IUserAgent
    {
        List<UserAgentModel> GetUserAgent(int size);

        List<BrowserModel> GetBrowser(List<BrowserModel> browserList);

        List<OSModel> GetOS(List<OSModel> osList);

        int InsertBrowser(string browserName);

        int InsertOS(string osName);

        int UpdateUserAgent(string userAgentXML);
    }
}

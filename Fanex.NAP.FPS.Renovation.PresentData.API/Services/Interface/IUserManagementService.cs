using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface
{
    public interface IUserManagementService
    {
        bool IsValidUser(string username, string password);
    }
}

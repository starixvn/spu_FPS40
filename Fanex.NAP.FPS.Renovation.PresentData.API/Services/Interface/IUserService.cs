using Fanex.NAP.FPS.Renovation.PresentData.API.Auth;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface
{
    interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}
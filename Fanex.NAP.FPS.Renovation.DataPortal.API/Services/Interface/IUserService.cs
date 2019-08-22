using Fanex.NAP.FPS.Renovation.DataPortal.API.Auth;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Interface
{
    public interface IUserService
    {
        Task RegisterAsync(string email, string password, string name);
        Task<JsonWebToken> LoginAsync(string email, string password);
    }
}
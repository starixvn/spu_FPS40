using Fanex.NAP.FPS.Renovation.Model;
using System;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Repositories
{
    public interface IUserRepository
    {
        Task<UserModel> GetAsync(string email);
        Task AddAsync(UserModel user);
    }
}
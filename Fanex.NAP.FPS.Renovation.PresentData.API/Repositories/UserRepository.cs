using System;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository() { }

        public async Task AddAsync(UserModel user)
        {
            throw new NotImplementedException();
        }     

        public async Task<UserModel> GetAsync(string email)
        {
            return GetUser();
        }

        private UserModel GetUser()
        {
            return new UserModel("test@test.com", "secret");
        }
    }
}
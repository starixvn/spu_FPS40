using System;
using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model.Auth;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Repositories
{
    public class UserRepository : IUserRepository
    {

        public async Task AddAsync(UserModel user)
        {
            // To Do: insert user to database
            // ...           
        }     

        public async Task<UserModel> GetAsync(string email)
        {
            // To Do: return user from database
            // ...

            return GetUser();
        }

        private UserModel GetUser()
        {
            return new UserModel("user1@email.com", "secret");
        }
    }
}
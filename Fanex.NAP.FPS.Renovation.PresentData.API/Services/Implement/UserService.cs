using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model;
using Fanex.NAP.FPS.Renovation.PresentData.API.Auth;
using Fanex.NAP.FPS.Renovation.PresentData.API.Repositories;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IJwtHandler _jwtHandler;

        public UserService(IUserRepository repository, IJwtHandler jwtHandler)
        {
            _repository = repository;
            _jwtHandler = jwtHandler;
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await _repository.GetAsync(email);
            if (user != null)
            {
                throw new System.Exception(string.Format("Email: '{0}' is already in use.", email));
            }
            user = new UserModel(email, name);
            //user.SetPassword(password, _encrypter);

            _repository.AddAsync(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user  = await _repository.GetAsync(email);

            if (user == null)
            {
                throw new System.Exception("Invalid credentials.");
            }

            return _jwtHandler.Create(user.Id);
        }
    }
}

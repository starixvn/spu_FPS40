using System.Threading.Tasks;
using Fanex.NAP.FPS.Renovation.Model.Auth;
using Fanex.NAP.FPS.Renovation.DataPortal.API.Auth;
using Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Interface;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Services.Implement
{
    public class UserService : IUserService
    {
        //private readonly IUserRepository _repository;
        //private readonly IJwtHandler _jwtHandler;
        //private readonly IEncrypter _encrypter;

        //public UserService(IUserRepository repository, IJwtHandler jwtHandler, IEncrypter encrypter)
        //{
        //    _repository = repository;
        //    _jwtHandler = jwtHandler;
        //    _encrypter = encrypter;
        //}

        public async Task RegisterAsync(string email, string password, string name)
        {
            //var user = await _repository.GetAsync(email);
            ////if (user != null)
            ////{
            ////    throw new System.Exception(string.Format("Email: '{0}' is already in use.", email));
            ////}
            //user = new UserModel(email, name);
            //user.SetPassword(password, _encrypter);

            //await _repository.AddAsync(user);
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            //var user  = await _repository.GetAsync(email);

            //if (user == null)
            //{
            //    throw new System.Exception("Invalid credentials.");
            //}

            //return _jwtHandler.Create(user.Id);

            return null;
        }
    }
}

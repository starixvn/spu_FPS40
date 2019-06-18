using System.Text;
using Fanex.NAP.FPS.Renovation.PresentData.API.Repositories;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Fanex.NAP.FPS.Renovation.PresentData.API.Auth
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            var section = configuration.GetSection("jwt");
            section.Bind(options);

            services.Configure<JwtOptions>(configuration.GetSection("jwt"));
            services.AddSingleton<IJwtHandler, JwtHandler>();

            var tokens = Encoding.UTF8.GetBytes(options.SecretKey);

            services.AddAuthentication().AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidIssuer = options.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
                };
            });
        }

        public static void AddUserService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            RegisterUser();
            //LoginAsync().ConfigureAwait(false);
        }

        public static void RegisterUser()
        {
            var email = "test@test.com";
            var password = "secret";
            var name = "test";

            var userRepository = new UserRepository();
            var jwtHandler = new JwtHandler(new JwtOptions());

            IUserService userService = new UserService(userRepository, jwtHandler);
            var jwt = userService.RegisterAsync(email, password, name);
        }

        public static async Task LoginAsync()
        {
            var email = "test@test.com";
            var password = "secret";

            var userRepository = new UserRepository();
            var jwtHandler = new JwtHandler(new JwtOptions());

            IUserService userService = new UserService(userRepository, jwtHandler);
            var jwt = await userService.LoginAsync(email, password);

        }
    }
}
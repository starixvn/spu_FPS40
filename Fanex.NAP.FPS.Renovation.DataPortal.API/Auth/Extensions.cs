using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Fanex.NAP.FPS.Renovation.DataPortal.API.Auth
{
    public static class Extensions
    {
        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //var options = new JwtOptions();
            //var section = configuration.GetSection("jwt");
            //section.Bind(options);

            //services.Configure<JwtOptions>(configuration.GetSection("jwt"));
            //services.AddSingleton<IJwtHandler, JwtHandler>();            

            //services.AddAuthentication().AddJwtBearer(cfg =>
            //{
            //    cfg.RequireHttpsMetadata = false;
            //    cfg.SaveToken = true;
            //    cfg.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateAudience = false,
            //        ValidIssuer = options.Issuer,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
            //    };
            //});
        }        
    }
}
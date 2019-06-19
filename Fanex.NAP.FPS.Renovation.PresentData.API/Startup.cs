using System.Linq;
using Fanex.Data;
using Fanex.Data.MySql;
using Fanex.NAP.FPS.Renovation.Model.Auth;
using Fanex.NAP.FPS.Renovation.PresentData.API.Auth;
using Fanex.NAP.FPS.Renovation.PresentData.API.Repositories;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Implement;
using Fanex.NAP.FPS.Renovation.PresentData.API.Services.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fanex.NAP.FPS.Renovation.PresentData.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddJwt(Configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IEncrypter, Encrypter>();

            var connections = Configuration.GetSection("ConnectionStrings").GetChildren();
            var connectionString = connections.ToDictionary(x => x.Key, x => new ConnectionConfiguration(x.Key, x.GetSection("ConnectionString").Value, x.GetSection("ProviderName").Value));

            var appData = Configuration.GetSection("AppSettings:AppDataPath").Value;

            DbSettingProviderManager
                .StartNewSession()
                .WithMySql()
                .Use(connectionString)
                .UseDefaultDbSettingProvider(resourcePath: appData)
                .Run();         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
        
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}

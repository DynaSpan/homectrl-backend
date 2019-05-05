using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeCTRL.Backend.Core.Auth;
using HomeCTRL.Backend.Core.Database;
using HomeCTRL.Backend.Core.Models;
using HomeCTRL.Backend.Features.Users;
using HomeCTRL.Backend.Features.Users.Repository;
using HomeCTRL.Backend.Features.Users.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HomeCTRL.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            
            //////////////////////////
            //    Database setup    //
            //////////////////////////
            services.AddScoped<IDatabaseFactory, DatabaseFactory>
                    (db => new DatabaseFactory(Configuration.GetSection("DatabaseConnection").Get<DatabaseSettings>()));

            ////////////////////////
            // JWT authentication //
            ////////////////////////
            var appSettings = appSettingsSection.Get<AppSettings>();
            var authServicesHelper = new AuthServicesHelper(services, appSettings.TokenSecret);

            authServicesHelper.ConfigureAuthenticationServices();

            ////////////////////////
            // Dependency inject. //
            ////////////////////////
            services.AddScoped<IAuthServicesHelper>(ash => authServicesHelper); 
            
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }

            app.UseAuthentication();
            app.UseMvc();

            ////////////////
            // AutoMapper //
            ////////////////
            Mapper.Initialize(cfg => {
                cfg.CreateMap<UserEntity, User>();
                cfg.CreateMap<User, UserEntity>();
            });
        }
    }
}

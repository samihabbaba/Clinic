using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Clinic.API.Installer;
using Clinic.API.Models;
using Clinic.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Clinic.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.InstallServicesAssembly(Configuration);
            services.AddAutoMapper(typeof(Startup));

        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var swaggerSettings = new SwaggerSettings();
            Configuration.GetSection(nameof(swaggerSettings)).Bind(swaggerSettings);
            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerSettings.JsonRoute;
            });
            app.UseSwaggerUI(setupAction => {
                setupAction.SwaggerEndpoint(swaggerSettings.UiEndPoint, swaggerSettings.Description);
                setupAction.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            // UseStaticFiles();
            // UseDefaultFiles();

            app.UseRouting();

            app.UseCors("DiscPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        //  private async Task CreateRoles(IServiceProvider serviceProvider)
        // {
        //     //adding customs roles : Question 1
        //     var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //     var UserManager = serviceProvider.GetRequiredService<UserManager<SystemUser>>();
        //     string[] roleNames = { "Admin", "Doctor", "Patient" };
        //     IdentityResult roleResult;

        //     foreach (var roleName in roleNames)
        //     {
        //         var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //         if (!roleExist)
        //         {
        //             //create the roles and seed them to the database: Question 2
        //             roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //         }
        //     }

        //     //Here you could create a super user who will maintain the web app
        //     var poweruser = new SystemUser
        //     {
        //         UserName = Configuration["AppSettings:UserName"],
        //         Email = Configuration["AppSettings:UserEmail"],
        //     };

        //     string userPWD = Configuration["AppSettings:UserPassword"];
        //     var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //     if(_user == null)
        //     {
        //         var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //         if (createPowerUser.Succeeded)
        //         {
        //             //here we tie the new user to the role : Question 3
        //             await UserManager.AddToRoleAsync(poweruser, "Admin");

        //         }
        //     }
        // }
        
    }
}

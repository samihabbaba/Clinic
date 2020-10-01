using System;
using API.Clinic.API.Contracts;
using API.Clinic.API.LoggingService;
using Clinic.API.Models;
using Clinic.API.Services.Main;
using Clinic.Services.Main;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.API.Installer
{
    public class ServicesInstaller: IInstaller
    {
        public void InstallServicesAssembly(IServiceCollection services, IConfiguration configuration)
        {
            var ConnectionString = configuration["connectionString:DataConnection"];
            services.AddDbContext<DataAccess.DataContext>(item =>
            item.UseSqlServer(ConnectionString, options => options.CommandTimeout(180)))
                .AddDefaultIdentity<SystemUser>(options => 
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataAccess.DataContext>();

            services.AddCors(options => options.AddPolicy("DiscPolicy",
                builder => 
                {
                    // for signalR you must use .SetIsOriginAllowed (host)=>true) with .AllowCredentials()
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyHeader();
                }));

            services.AddScoped<ISystemUserService, SystemUserService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddControllers();
        }
    }
}
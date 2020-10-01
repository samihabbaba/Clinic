using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Clinic.API.Installer
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServicesAssembly(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Title = "Clinic API",
                        Version = "v1",
                        Description = "Test API for Clinic.",
                        Contact = new OpenApiContact()
                        {
                            Email = "sami_habbaba30@hotmail.com",
                            Name = "Sami Habbaba",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer",new string[0]}
                };
                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Autherization Header using the bearer Scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }},new List<string>()}
                });

                var xmlFile = $"Clinic.API.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                setupAction.IncludeXmlComments(xmlPath);
            });
        }
    }
}
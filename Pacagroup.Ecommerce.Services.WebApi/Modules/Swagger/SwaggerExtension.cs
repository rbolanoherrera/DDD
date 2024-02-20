using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;

namespace Pacagroup.Ecommerce.Services.WebApi.Modules.Swagger
{
    /// <summary>
    /// Metodo de extension de swagger para quitar todo ese codigo del archivo startup.cs
    /// </summary>
    public static class SwaggerExtension
    {
        /// <summary>
        /// Agregar la configuracion personalizada de Swagger al proyecto
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            //services.AddSwaggerGen();//tambien funciona agregando solo esta línea
            services.AddSwaggerGen(sw =>
            {
                sw.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Implementando Arquitectura DDD en .NET",
                    TermsOfService = new System.Uri("https://github.com/rbolanoherrera/DDD"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Rafael Bolaños Herrera",
                        Email = "ralfs1@hotmail.com",
                        Url = new System.Uri("https://github.com/rbolanoherrera/DDD")
                    },
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "Open Source",
                        Url = new System.Uri("https://github.com/rbolanoherrera/DDD")
                    }
                });

                //LoggerText.writeLog("antes de GetName().Name}.xml");

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), xmlFile);
                sw.IncludeXmlComments(xmlPath);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                //Agergar seguridad a swagger para los metodos protegidos con Authorize
                sw.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

                sw.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        securityScheme, new string[]{}
                    }
                });

            });

            return services;
        }
    }
}
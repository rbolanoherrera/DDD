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

                //LoggerText.writeLog("despues de w.IncludeXmlComments");

                //Agergar seguridad a swagger para los metodos protegidos con Authorize
                sw.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Authorization by API key",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Name = "Authorization"
                });

                sw.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            //Name = "Authorization",
                            //In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[]{}
                    }
                });

            });

            return services;
        }
    }
}
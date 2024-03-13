﻿using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Pacagroup.Ecommerce.Services.WebApi.Modules.Versioning
{
    /// <summary>
    /// Clase donde se configura el versionamiento de la App
    /// </summary>
    public static class VersioningExtensions
    {
        /// <summary>
        /// Agregar al contenedor de dependencias la configuración del versionamiento
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(v =>
            {
                v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.ReportApiVersions = true;
                v.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            });

            //para que swagger pueda trabajar con el versionamiento
            services.AddVersionedApiExplorer(v =>
            {
                v.GroupNameFormat = "'v'VVV";
            });

            return services;
        }
    }
}
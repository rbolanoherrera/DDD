using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Pacagroup.Ecommerce.Application.Interface;
using Pacagroup.Ecommerce.Application.Main;
using Pacagroup.Ecommerce.Domain.Core;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Infrastructure.Data;
using Pacagroup.Ecommerce.Infrastructure.Interface;
using Pacagroup.Ecommerce.Infrastructure.Repository;
using Pacagroup.Ecommerce.Services.WebApi.Helpers;
using Pacagroup.Ecommerce.Transversal.Common;
using Pacagroup.Ecommerce.Transversal.Mapper.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pacagroup.Ecommerce.Services.WebApi
{
    public class Startup
    {
        readonly string myCORSPolicy = "policyAPIDDD";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddBuilders();//del proyecto Mapper

            services.AddCors(options => options.AddPolicy(myCORSPolicy, 
                builder => builder.WithOrigins(Configuration["OriginsCORS"])
                .AllowAnyHeader()
                .AllowAnyMethod()
            ));

            services.AddMvc();

            //services.AddJsonOptions(options =>
            //                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            //);

            var settingsJWT = Configuration.GetSection("ConfigJWT");
            services.Configure<AppSettings>(settingsJWT);

            services.AddSingleton<IConfiguration>(Configuration);
            
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();

            services.AddScoped<ICustomerApplication, CustomerApplication>();
            services.AddScoped<ICustomerDomain, CustomerDomain>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddScoped<IUserDomain, UserDomain>();
            services.AddScoped<IUserRepository, UserRepository>();

            byte[] key = Encoding.ASCII.GetBytes(settingsJWT.GetValue<string>("Secret"));
            string issuer = settingsJWT.GetValue<string>("Issuer");
            string audience = settingsJWT.GetValue<string>("Audience");

            services.AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        int userId = int.Parse(context.Principal.Identity.Name);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");

                        return Task.CompletedTask;
                    }
                };

                x.RequireHttpsMetadata = false;
                x.SaveToken = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

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

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(Directory.GetCurrentDirectory(), xmlFile);
                sw.IncludeXmlComments(xmlPath);

                //Agergar seguridad a swagger para los metodos protegidos con Authorize
                sw.AddSecurityDefinition("Authorization", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
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
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Authorization",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
                //app.UseSwaggerUI(sw =>
                //{
                //    sw.SwaggerEndpoint("/swagger/v1/swagger.json", "My API Arquitectura DDD .NET");
                //});
            }

            //app.UseCors(c =>
            //{
            //    c.AllowAnyOrigin();
            //    c.AllowAnyHeader();
            //    c.AllowAnyMethod();
            //});
            app.UseCors(myCORSPolicy);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

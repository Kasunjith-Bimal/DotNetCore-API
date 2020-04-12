using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using TESTAPI.Authorization;
using TESTAPI.Extention;
using TESTAPI.Options;
using TESTAPI.Services;

namespace TESTAPI.Instrallers
{
    public class MvcInstraller : IInstraller
    {
        public void InstallServices(IServiceCollection services, IConfiguration Configuration)
        {
            var JwtSettings = new JwtSettings();
            Configuration.Bind(nameof(JwtSettings), JwtSettings);

            services.AddSingleton(JwtSettings);
           
            services.AddScoped<IPostService, PostService>();


            services.AddMvc(x=> { x.EnableEndpointRouting = false; }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            

          
            var TokenValidationParametes = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtSettings.Issuer,
                ValidAudience = JwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSettings.Secret)),

            };

            services.AddSingleton(TokenValidationParametes);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(x => {
                x.SaveToken = true;
                x.TokenValidationParameters = TokenValidationParametes;


            });

            services.AddAuthorization(Options => {

                Options.AddPolicy("tagViewer", builder => builder.RequireClaim("tag.view", "true"));
                Options.AddPolicy("WorkMyComapny", policy =>
                {
                    policy.AddRequirements(new Requirement("kasunysoft.com"));

                });
            });

            services.AddSingleton<IAuthorizationHandler, Handler>();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "V1" });



                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {

                    Description = "JWT Authentication header using bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey

                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {new OpenApiSecurityScheme{ Reference = new OpenApiReference
                        {
                        Id ="Bearer",
                        Type = ReferenceType.SecurityScheme

                    }},new List<string>()}
                });

            });



        }
    }
}

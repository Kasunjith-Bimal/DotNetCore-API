using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Info { Title = "API", Version = "V1" });

                

                x.AddSecurityDefinition("Bearer", new ApiKeyScheme {

                    Description = "JWT Authentication header using bearer scheme",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"

                });

                x.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

            });

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


        }
    }
}

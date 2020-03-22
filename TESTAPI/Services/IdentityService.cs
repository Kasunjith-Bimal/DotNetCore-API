using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TESTAPI.Domain;
using TESTAPI.Options;

namespace TESTAPI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _JwtSttings;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings JwtSttings)
        {
            _userManager = userManager;
            _JwtSttings = JwtSttings;
           
        }

        public async Task<AuthenticationResult> RegistrationAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] {"üser With the email address already exists"}

                };
            }

            var newUsser = new IdentityUser
            {
                Email = email,
                UserName = email
            };

            var createUser = await _userManager.CreateAsync(newUsser, password);

            if (!createUser.Succeeded)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = createUser.Errors.Select(x => x.Description)
                };
            }

           // var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_JwtSttings.Secret);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, newUsser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUsser.Email),
                    new Claim("Id", newUsser.Id),

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256)
            };


            var token = new JwtSecurityToken(_JwtSttings.Issuer,_JwtSttings.Issuer,expires: DateTime.Now.AddHours(2), signingCredentials: tokenDescription.SigningCredentials);

            return new AuthenticationResult
            {
                Sucess = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }
    }
}

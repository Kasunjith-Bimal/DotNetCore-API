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

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = new[] { "User does not exits" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);


            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {

                    ErrorMessage = new[] { "User Password conbination is Wrong" }
                };
            }

            return GenerateAuthenticationResultForUser(user);
        }

        public async Task<AuthenticationResult> RegistrationAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    ErrorMessage = new[] { "üser With the email address already exists" }

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
            return GenerateAuthenticationResultForUser(newUsser);

        }

        private AuthenticationResult GenerateAuthenticationResultForUser(IdentityUser user)
        {
            var key = Encoding.ASCII.GetBytes(_JwtSttings.Secret);
            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),

                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };


            // var jsonuser = new { id = user.Id };


            var token = new JwtSecurityToken(_JwtSttings.Issuer, _JwtSttings.Issuer, claims: tokenDescription.Subject.Claims, null, expires: DateTime.Now.AddHours(2), signingCredentials: tokenDescription.SigningCredentials);

           // token.Payload["User"] = jsonuser;

            return new AuthenticationResult
            {
                Sucess = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}

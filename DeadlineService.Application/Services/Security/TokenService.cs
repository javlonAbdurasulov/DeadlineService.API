using DeadlineService.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Services.Security
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public JwtToken GenerateJwtToken(string UserName)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            //var tokenKey = "Aa12@#aA";
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization")["SecretKey"] ?? "Aa12@#aA");
            var tokenIssuer = _configuration.GetSection("Authorization")["Issuer"];
            var tokenAudience = _configuration.GetSection("Authorization")["Audience"];
            var tokenExpiretTime = Convert.ToInt32(_configuration.GetSection("Authorization")["ExpiretTime"]);

            var claims = new List<Claim> { new Claim(ClaimTypes.Role, UserName), new Claim(ClaimTypes.Name, UserName) };
            var jwt = new JwtSecurityToken(
                    issuer: tokenIssuer,
                    audience: tokenAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiretTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256));

            var refreshToken = RefreshToken(UserName);

            return new JwtToken { AccessToken = tokenHandler.WriteToken(jwt), RefreshToken = refreshToken };

        }
        public string RefreshToken(string UserName)
        {
            //использовать как-то username
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}

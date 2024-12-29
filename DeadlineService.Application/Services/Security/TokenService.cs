using DeadlineService.Application.Interfaces.Repostitories;
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
        private readonly IUserService _userService;
        public TokenService(IConfiguration configuration,IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        public async Task<ResponseModel<JwtToken>> CreateJwtToken(string UserName)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings")["SecretKey"] ?? "Aa12@#aA");
            var tokenIssuer = _configuration.GetSection("JWTSettings")["Issuer"];
            var tokenAudience = _configuration.GetSection("JWTSettings")["Audience"];
            var tokenExpiretTime = Convert.ToInt32(_configuration.GetSection("JWTSettings")["ExpiretTime"]);

            var claims = new List<Claim> { new Claim(ClaimTypes.Role, UserName), new Claim(ClaimTypes.Name, UserName) };
            var jwt = new JwtSecurityToken(
                    issuer: tokenIssuer,
                    audience: tokenAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiretTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256));
            //await
            var refreshToken = RefreshToken(UserName);

            return new(new JwtToken { AccessToken = tokenHandler.WriteToken(jwt), RefreshToken = refreshToken });

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
            var jwtSettings = _configuration.GetSection("JWTSettings");
            var tokenValidateParametrs = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (_configuration.GetSection("JWTSettings")["SecretKey"] ?? "Aa12@#aA")),
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token,tokenValidateParametrs, out securityToken);
            var jwtTokenSecurity = securityToken as JwtSecurityToken;

            if (jwtTokenSecurity is null 
                || !jwtTokenSecurity.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
            {

            }
            return principal;
        }
    }
}

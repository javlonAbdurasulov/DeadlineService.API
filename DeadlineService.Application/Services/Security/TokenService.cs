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
        private readonly IRoleRepository _roleRepository;
        public TokenService(IConfiguration configuration, IUserService userService, IRoleRepository roleRepository)
        {
            _configuration = configuration;
            _userService = userService;
            _roleRepository = roleRepository;
        }
        public async Task<ResponseModel<JwtToken>> CreateJwtToken(string UserName)
        {
            var user = await _userService.GetByUsernameAsync(UserName);
            var role = await _roleRepository.GetAllWithUserAsync();
            var roleUser = role.FirstOrDefault(x => x.Users.FirstOrDefault(x => x.Username == UserName).Username == UserName);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings")["SecretKey"] ?? "Aa12@#aA");
            var tokenIssuer = _configuration.GetSection("JWTSettings")["Issuer"];
            var tokenAudience = _configuration.GetSection("JWTSettings")["Audience"];
            var tokenExpiretTime = Convert.ToInt32(_configuration.GetSection("JWTSettings")["ExpiretTime"]);

            var claims = new List<Claim> { new Claim(ClaimTypes.Role, roleUser.Name), new Claim(ClaimTypes.Name, UserName) };
            var jwt = new JwtSecurityToken(
                    issuer: tokenIssuer,
                    audience: tokenAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiretTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256));
            var refreshToken = GenerateRefreshToken();
            //await и обновить пользователя в базе его refreshToken
            user.Result.RefreshToken = refreshToken;
            user.Result.RefreshTokenExpireTimeUtc = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWTSettings")["ExpiretTime"]));
            user = await _userService.UpdateUser()

            return new(new JwtToken { AccessToken = tokenHandler.WriteToken(jwt), RefreshToken = refreshToken });

        }
        public Task<ResponseModel<JwtToken>> RefreshToken(JwtToken jwtToken)
        {

            throw new NotImplementedException();
        }
        private string GenerateRefreshToken()
        {
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

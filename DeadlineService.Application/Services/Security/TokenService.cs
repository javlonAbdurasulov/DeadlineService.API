using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.User;
using DeadlineService.Domain.Models.Entity;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<ResponseModel<JwtToken>> CreateJwtToken(int UserId,string UserName)
        {
            var accesToken = await GenerateAccessToken(UserId, UserName);
            var refreshToken = GenerateRefreshToken();


            UserUpdateDTO userUpdateDto = new() {
                UserId = UserId,
                RefreshToken = refreshToken,
                RefreshTokenExpireTimeUtc = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration.GetSection("JWTSettings")["ExpiretTime"]))
            };
            var user = await _userService.UpdateUser(userUpdateDto);
            if(user.Result == false)
            {
                return new(user.Error);
            }

            return new(new JwtToken { AccessToken = accesToken, RefreshToken = refreshToken });
        }
        public async Task<ResponseModel<JwtToken>> RefreshToken(JwtToken jwtToken)
        {
            var principal = GetPrincipalFromExpiredToken(jwtToken.AccessToken);
            if(principal is null)
            {
                return new("Ошибка расшифровки токена! неправильный");
            }
            var username = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;

            var user = await _userService.GetByUsernameAsync(username);
            if(user is null || user.Result.RefreshToken != jwtToken.RefreshToken ||
                user.Result.RefreshTokenExpireTimeUtc < DateTime.UtcNow)
            {
                return new("время использования RefreshToken истёк");
            }

            var userId = Convert.ToInt32(principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return await CreateJwtToken(userId, username);
        }
        private async Task<string> GenerateAccessToken(int UserId,string UserName)
        {
            var role = await _roleRepository.GetAllWithUserAsync();
            var roleUser = role.FirstOrDefault(x => x.Users.FirstOrDefault(x => x.Username == UserName).Username == UserName);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration.GetSection("JWTSettings")["SecretKey"] ?? "Aa12@#aA");
            var tokenIssuer = _configuration.GetSection("JWTSettings")["Issuer"];
            var tokenAudience = _configuration.GetSection("JWTSettings")["Audience"];
            var tokenExpiretTime = Convert.ToInt32(_configuration.GetSection("JWTSettings")["ExpiretTime"]);

            var claims = new List<Claim> { new Claim(ClaimTypes.Role, roleUser.Name), new Claim(ClaimTypes.Name, UserName),
                                            new Claim(ClaimTypes.NameIdentifier, $"{UserId}") };
            var jwt = new JwtSecurityToken(
                    issuer: tokenIssuer,
                    audience: tokenAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(tokenExpiretTime)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256));
            
            return tokenHandler.WriteToken(jwt);
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
                return null;
            }
            return principal;
        }
    }
}

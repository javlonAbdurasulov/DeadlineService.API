using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface ITokenService
    {
        JwtToken GenerateJwtToken(string UserName);
        string RefreshToken(string UserName);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

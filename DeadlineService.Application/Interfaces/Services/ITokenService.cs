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
        Task<ResponseModel<JwtToken>> CreateJwtToken(int UserId,string UserName);
        Task<ResponseModel<JwtToken>> RefreshToken(JwtToken jwtToken);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

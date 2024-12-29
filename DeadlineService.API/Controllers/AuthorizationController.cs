using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.User;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Net;

namespace DeadlineService.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        public AuthorizationController(IUserService userService,
           IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _userService = userService;
        }
        [HttpPost]
        public async Task<ResponseModel<User>> Registration(RegisterUser registerUser)
        {
            if (registerUser.ConfirmPassword != registerUser.Password)
            {
                return new ResponseModel<User>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "Password and Confirm Password do not match.",
                    Result = null
                };
            }

            return await _authorizationService.RegistrationAsync(registerUser);
        }
        [HttpPost]
        public async Task<ResponseModel<UserGetDTO>> Login(LoginUser loginUser)
        {
            return await _authorizationService.LoginAsync(loginUser);
        }
    }
}

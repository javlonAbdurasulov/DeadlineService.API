using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace DeadlineService.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheService _redisCache;
        public UserController(IUserService userService,
            IRedisCacheService redisCache)
        {
            _redisCache=redisCache;
            _userService =userService;
        }
        [HttpGet]
        public async Task<ResponseModel<UserGetDTO>> UserById(int id)
        {       
            string cacheKey = $"User:{id}";

            var cachedUserData = await _redisCache.GetAsync(cacheKey);
            if (cachedUserData != null)
            {

                var cachedUser = JsonSerializer.Deserialize<ResponseModel<UserGetDTO>>(cachedUserData);
                if (cachedUser != null)
                    return cachedUser;
            }

            var userResponse = await _userService.GetUserByIdAsync(id);
            if (userResponse != null )
            {
                var userDataToCache=JsonSerializer.Serialize(userResponse);
                await _redisCache.SetAsync(cacheKey, UTF8Encoding.UTF8.GetBytes(userDataToCache));
            }
            return userResponse;
        }
        [HttpGet]
        public async Task<ResponseModel<UserGetDTO>> UserByEmail(string email)
        {
            string cacheKey = $"User:{email}";

            var cachedUserData = await _redisCache.GetAsync(cacheKey);
            if (cachedUserData != null)
            {
                var cachedUser = JsonSerializer.Deserialize<UserGetDTO>(cachedUserData);
                if (cachedUser != null)
                    return new ResponseModel<UserGetDTO> { Result = cachedUser };
            }

            var userResponse = await _userService.GetByEmailAsync(email);
            if (userResponse != null)
            {
                var userDataToCache = JsonSerializer.Serialize(userResponse);
                byte[] inBytes = UTF8Encoding.UTF8.GetBytes(userDataToCache);
                await _redisCache.SetAsync($"User:{email}", inBytes);
            }
            return userResponse;
        }
        [HttpPost]
        public async Task<ResponseModel<UserGetDTO>> Registration(RegisterUser registerUser)
        {
            if (registerUser.ConfirmPassword != registerUser.Password)
            {
                return new ResponseModel<UserGetDTO>()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Error = "Password and Confirm Password do not match.",
                    Result = null
                };
            }

            return await _userService.RegistrationAsync(registerUser);
        }
        [HttpPost]
        public async Task<ResponseModel<UserGetDTO>> Login(LoginUser loginUser) {
            return await _userService.LoginAsync(loginUser);
        }
    }
}

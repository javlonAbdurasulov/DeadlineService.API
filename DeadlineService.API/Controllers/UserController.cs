using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.User;
using DeadlineService.Domain.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DeadlineService.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRedisCacheService _redisCache;
        public UserController(IUserService userService,
            IRedisCacheService redisCache)
        {
            _redisCache = redisCache;
            _userService = userService;
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
            if (userResponse != null)
            {
                var userDataToCache = JsonSerializer.Serialize(userResponse);
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
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<User>>> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return allUsers;
        }
        [HttpGet]
        public async Task<ResponseModel<IEnumerable<User>>> GetAllWithAllInformationAsync()
        {
            var allUsers = await _userService.GetAllWithAllInformationAsync();
            return allUsers;
        }
        [HttpPatch]
        public async Task<ResponseModel<IEnumerable<UserGetDTO>>> UpdateUser(string username,string email)
        {
            var allUsers = await _userService.GetAllUsers();
            return new ResponseModel<IEnumerable<UserGetDTO>>();
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

            return await _userService.RegistrationAsync(registerUser);
        }
        [HttpPost]
        public async Task<ResponseModel<UserGetDTO>> Login(LoginUser loginUser)
        {
            return await _userService.LoginAsync(loginUser);
        }
    }
}

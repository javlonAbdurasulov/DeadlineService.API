using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPersonalInfoRepository _personalInfo;
        private readonly IRedisCacheService _cache;
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IPersonalInfoRepository personalInfo,
            IRedisCacheService appCache)
        {
            _personalInfo = personalInfo;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _cache = appCache;
        }

        public async Task<ResponseModel<UserGetDTO>> CreateAsync(RegisterUser registerUser)
        {
            bool UsernameIsNotHave = await _userRepository.CheckUsername(registerUser.Username);
            if (!UsernameIsNotHave)
            {
                return new("username уже существует");
            }
            User user = new User(registerUser.Username, _passwordHasher.StringToHash(registerUser.Password));
            user = await _userRepository.CreateAsync(user);
            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Id = user.Id,
                PersonalInfoId = user.Id,
                Username = user.Username
            };
            return new(userGetDTO);
        }

        public async Task<ResponseModel<UserGetDTO>> GetByEmailAsync(string email)
        {
            if(string.IsNullOrEmpty(email)) return new("email is empty");
            User userWithEmail=await _userRepository.GetByEmail(email);
            if(userWithEmail == null)
            {
                return new("Пользователь с таким email не сушествует!");
            }
            UserGetDTO userGetDTO = new UserGetDTO()
            {
                PersonalInfoId = userWithEmail.PersonalInfoId,
                Username = userWithEmail.Username,
                Id = userWithEmail.Id
            };
            return new(userGetDTO);
        }
    }
}

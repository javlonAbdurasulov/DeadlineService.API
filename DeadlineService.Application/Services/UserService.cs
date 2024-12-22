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
        private readonly IRedisCacheService _cache;
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRedisCacheService appCache)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _cache = appCache;
        }

        public async Task<ResponseModel<UserGetDTO>> RegistrationAsync(RegisterUser registerUser)
        {
            User UsernameIsHave = await _userRepository.GetByUsernameAsync(registerUser.Username);
            if (UsernameIsHave!=null)
            {
                return new("username уже существует");
            }
            User user = new User(registerUser.Username, _passwordHasher.StringToHash(registerUser.Password));
            user = await _userRepository.CreateAsync(user);
            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Id = user.Id,
                PersonalInfoId = user.PersonalInfoId,
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

        public async Task<ResponseModel<UserGetDTO>> LoginAsync(LoginUser loginUser)
        {
            User user = await _userRepository.GetByUsernameAsync(loginUser.Username);
            if(user == null)
            {
                return new("Пользователь с таким username не существует!");
            }
            else
            {
                if(user.PasswordHash == _passwordHasher.StringToHash(loginUser.Password))
                {
                    UserGetDTO userGetDTO = new UserGetDTO()
                    {
                        PersonalInfoId = user.PersonalInfoId,
                        Username = user.Username,
                        Id = user.Id
                    };
                    return new(userGetDTO);
                }
                else
                {
                    return new("Неправильный пароль!");
                }
            }
        }

        public async Task<ResponseModel<UserGetDTO>> GetUserByIdAsync(int Id)
        {
            User? user = await _userRepository.GetById(Id);
            if(user == null)
            {
                return new("User с таким Id не найден!");
            }
            UserGetDTO userGetDTO = new()
            {
                Id = user.Id,
                PersonalInfoId = user.PersonalInfoId,
                Username = user.Username
            };
            return new(userGetDTO);
        }
    }
}

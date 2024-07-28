using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.User;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IPersonalInfoRepository personalInfo)
        {
            _personalInfo= personalInfo;
            _passwordHasher = passwordHasher;
            _userRepository=userRepository;

        }

        public async Task<User> GetByEmail(string email)
        {
            if(string.IsNullOrEmpty(email)) throw new ArgumentNullException("email is incorrect");

            User userWithEmail=await _userRepository.GetByEmail(email);

            return userWithEmail;

        }
    }
}

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
            var allInfo= await _personalInfo.GetAllAsync();

           PersonalInfo? personalInfo = allInfo.FirstOrDefault(x => x.Email == email);

            if (personalInfo != null) return null;

            return personalInfo.User;

        }
    }
}

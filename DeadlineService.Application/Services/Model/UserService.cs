using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.User;
using MyApp.Services;
using System.Net.Mail;

namespace DeadlineService.Application.Services.Model
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
    
        private readonly IRedisCacheService _cache;
      

        public UserService(
            IUserRepository userRepository,
            IRedisCacheService appCache
          )
        {
            _userRepository = userRepository;
            _cache = appCache;
        }

        public async Task<ResponseModel<UserGetDTO>> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return new("email is empty");

            User userWithEmail = await _userRepository.GetByEmailAsync(email);

            if (userWithEmail == null)
                return new("Пользователь с таким email не сушествует!");

            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Username = userWithEmail.Username,
                Id = userWithEmail.Id
            };
            return new(userGetDTO);
        }

        public async Task<ResponseModel<UserGetDTO>> GetUserByIdAsync(int Id)
        {
            User? user = await _userRepository.GetByIdAsync(Id);
            if (user == null)
                return new ResponseModel<UserGetDTO>("User с таким Id не найден!");

            UserGetDTO userGetDTO = new()
            {
                Id = user.Id,
                Username = user.Username
            };
            return new ResponseModel<UserGetDTO>(userGetDTO);
        }
   
        public async Task<ResponseModel<IEnumerable<User>>> GetAllUsers()
        {
            IEnumerable<User> allUsers = await _userRepository.GetAllAsync();

            return new ResponseModel<IEnumerable<User>>(allUsers);
        }

        public async Task<ResponseModel<IEnumerable<User>>> GetAllWithAllInformationAsync()
        {
            var allUsers = await _userRepository.GetAllWithAllInformationAsync();
            return new ResponseModel<IEnumerable<User>>(allUsers);
        }   

        public async Task<ResponseModel<User>> UpdateUser(string username, string email)
        {
            var userNew = await _userRepository.GetByUsernameAsync(username);
            if (username == null) return new ResponseModel<User>("Пользователь с таким аддресом электронной почты не нашлось.");
            await _userRepository.UpdateAsync(userNew);
            return new ResponseModel<User>(userNew);
        }
    }
}

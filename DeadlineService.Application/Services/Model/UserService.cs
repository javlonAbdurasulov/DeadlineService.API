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
        private readonly IPersonalInfoService _personalInfoService;
      

        public UserService(
            IUserRepository userRepository,
            IRedisCacheService appCache,
            IPersonalInfoService personalInfoService
          )
        {
            _personalInfoService= personalInfoService ?? throw new ArgumentNullException(nameof(personalInfoService));
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
        public async Task<ResponseModel<UserGetDTO>> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username)) return new("email is empty");

            User userWithUsername = await _userRepository.GetByUsernameAsync(username);

            if (userWithUsername == null)
                return new("Пользователь с таким email не сушествует!");

            UserGetDTO userGetDTO = new UserGetDTO()
            {
                Username = userWithUsername.Username,
                Id = userWithUsername.Id,
                RefreshToken = userWithUsername.RefreshToken,
                RefreshTokenExpireTimeUtc = userWithUsername.RefreshTokenExpiryTimeUtc
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

        public async Task<ResponseModel<bool>> UpdateUser(UserUpdateDTO user)
        {
            var userNew = await _userRepository.GetByIdAsync(user.UserId);

            if (userNew == null) 
                return new ResponseModel<bool>("Пользователь с таким идентификатором не нашлось");

            var personalInfo =await  _personalInfoService.GetAllPersonalInfoAsync();
            
          var userForUpdate=  new PersonalInfoUpdateDTO()
            {
              Id= personalInfo.Result.FirstOrDefault(x => x.UserId == user.UserId).UserId,
                Description = user.Description,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Photo = user.Photo,
            };
            
            await _personalInfoService.UpdatePersonalInfoAsync(userForUpdate);

            await _userRepository.UpdateAsync(userNew);

            return new ResponseModel<bool>(true);
        }
    }
}

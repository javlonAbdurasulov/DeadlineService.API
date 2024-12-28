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
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRedisCacheService _cache;
        private readonly MailgunService _mailGunService;
        private readonly IPersonalInfoService _personalInfoService;
        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRedisCacheService appCache,
            MailgunService mailgunService,
            IPersonalInfoService personalInfoService)
        {
            _mailGunService = mailgunService;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _cache = appCache;
            _personalInfoService=personalInfoService;
        }

        public async Task<ResponseModel<User>> RegistrationAsync(RegisterUser registerUser)
        {
            User UsernameIsHave = await _userRepository.GetByUsernameAsync(registerUser.Username);

            if (UsernameIsHave != null)
                return new ResponseModel<User>("Пользователь с данным именем уже существует");

            if (registerUser.ConfirmPassword != registerUser.Password)
                return new ResponseModel<User>("Пароль и подтверждение пароля должны совпадать между собой");

            User user = new User(registerUser.Username, _passwordHasher.StringToHash(registerUser.Password));

            user = await _userRepository.CreateAsync(user);


            return new(user);
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

        public async Task<ResponseModel<UserGetDTO>> LoginAsync(LoginUser loginUser)
        {
            User user = await _userRepository.GetByUsernameAsync(loginUser.Username);

            if (user == null)
                return new("Пользователь с таким username не существует!");

            if (user.PasswordHash == _passwordHasher.StringToHash(loginUser.Password))
            {
                UserGetDTO userGetDTO = new UserGetDTO()
                {
                    Username = user.Username,
                    Id = user.Id
                };
                return new ResponseModel<UserGetDTO>(userGetDTO);
            }
            //Если пароль не совпал который в базе 
            return new ResponseModel<UserGetDTO>("Неправильный пароль!");
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
        public async Task SendConfirmationEmail(string userMail,string confirmationLink)
        { 
            var subject = "confirm your email";
            var body= $"<p>Click the link to confirm your email: <a href='{confirmationLink}'>Confirm Email</a></p>";

            await _mailGunService.SendEmailAsync(userMail, subject, body);
        }
        public async Task ResetMailPassword(string usermail,string resetLink)
        {
            var subject = "reset your password";
            var body = $"<p>Click the link to confirm your email:<a href-'{resetLink}'> Reset Password</a></p>";
          //  await _mailGunService.ResetPasswordEmailAsync(usermail, subject, body);
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
        public async Task<ResponseModel<User>> UpdateUser(string username, PersonalInfoCreateDTO obj)
        {
            var userNew = await _userRepository.GetByUsernameAsync(username);
            if (username == null) return new ResponseModel<User>("Пользователь с таким аддресом электронной почты не нашлось.");
            await _userRepository.UpdateAsync(userNew);
            return new ResponseModel<User>(userNew);
        }
    }
}

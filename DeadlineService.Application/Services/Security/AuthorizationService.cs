using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.DTOs.User;
using MyApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Services.Security
{
    internal class AuthorizationService:IAuthorizationService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly MailgunService _mailGunService;
        public AuthorizationService(IUserRepository userRepository, IPasswordHasher passwordHasher, MailgunService mailgunService)
        {
            _userRepository = userRepository;
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
        public async Task SendConfirmationEmail(string userMail, string confirmationLink)
        {
            var subject = "confirm your email";
            var body = $"<p>Click the link to confirm your email: <a href='{confirmationLink}'>Confirm Email</a></p>";

            await _mailGunService.SendEmailAsync(userMail, subject, body);
        }
        public async Task ResetMailPassword(string usermail, string resetLink)
        {
            var subject = "reset your password";
            var body = $"<p>Click the link to confirm your email:<a href-'{resetLink}'> Reset Password</a></p>";
            //  await _mailGunService.ResetPasswordEmailAsync(usermail, subject, body);
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
    }
}

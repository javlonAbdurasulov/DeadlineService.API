using DeadlineService.Domain.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IAuthorizationService
    {
        public  Task<ResponseModel<UserGetDTO>> LoginAsync(LoginUser loginUser);
        public  Task SendConfirmationEmail(string userMail, string confirmationLink);
        public  Task ResetMailPassword(string usermail, string resetLink);
        public  Task<ResponseModel<User>> RegistrationAsync(RegisterUser registerUser);
    }
}

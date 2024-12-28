using DeadlineService.Domain.Models.DTOs.User;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<ResponseModel<UserGetDTO>> GetByEmailAsync(string email);
        public Task<ResponseModel<User>> RegistrationAsync(RegisterUser registerUser);
        public Task<ResponseModel<UserGetDTO>> LoginAsync(LoginUser loginUser);
        public Task<ResponseModel<UserGetDTO>> GetUserByIdAsync(int Id);
        public Task<ResponseModel<IEnumerable<User>>> GetAllUsers();
        public Task<ResponseModel<IEnumerable<User>>> GetAllWithAllInformationAsync();
        public Task<ResponseModel<User>> UpdateUser(string username, string email);
        public Task SendConfirmationEmail(string userMail, string confirmationLink);

    }
}

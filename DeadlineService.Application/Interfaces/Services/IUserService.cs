using DeadlineService.Domain.Models.DTOs.User;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<ResponseModel<UserGetDTO>> GetByEmailAsync(string email);
        public Task<ResponseModel<UserGetDTO>> GetUserByIdAsync(int Id);
        public Task<ResponseModel<IEnumerable<User>>> GetAllUsers();
        public Task<ResponseModel<IEnumerable<User>>> GetAllWithAllInformationAsync();
        public Task<ResponseModel<bool>> UpdateUser(UserUpdateDTO user);
        public Task<ResponseModel<UserGetDTO>> GetByUsernameAsync(string username);
    }
}

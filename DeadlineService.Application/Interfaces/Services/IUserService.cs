using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IUserService 
    {
        public Task<ResponseModel<UserGetDTO>> GetByEmailAsync(string email);
        public Task<ResponseModel<UserGetDTO>> RegistrationAsync(RegisterUser registerUser);
        public Task<ResponseModel<UserGetDTO>> LoginAsync(RegisterUser loginUser);
        public Task<ResponseModel<UserGetDTO>> GetUserByIdAsync(int Id);

    }
}

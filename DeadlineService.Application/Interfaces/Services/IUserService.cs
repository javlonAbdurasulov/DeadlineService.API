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
     public Task<User> GetByEmail (string email);
    }
}

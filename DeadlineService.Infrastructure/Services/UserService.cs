using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly DSDbContext _db;
        public UserService(DSDbContext db)
        {
            _db=db;
        }
        public async Task<bool> CreateAsync(User obj)
        {
            await _db.Users.AddAsync(obj);
            int result=  await _db.SaveChangesAsync();
            return result > 0;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User obj)
        {
            throw new NotImplementedException();
        }
    }
}

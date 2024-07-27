using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;

namespace DeadlineService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly DSDbContext _db;
        public UserService(DSDbContext db)
        {
            _db=db;
        }
        public async Task<User> CreateAsync(User obj)
        {
            await _db.Users.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateAsync(User obj)
        {
            throw new NotImplementedException();
        }
    }
}

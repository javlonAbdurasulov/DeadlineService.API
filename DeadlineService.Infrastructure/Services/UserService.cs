using DeadlineService.Application.Interfaces.Services;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> DeleteAsync(int id)
        {
            User obj= await GetById(id);
            _db.Users.Remove(obj);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var result = await _db.Users.ToListAsync();
            return result;
        }

        public async Task<User?> GetById(int id)
        {
            User? obj = await _db.Users.FirstOrDefaultAsync(x=>x.Id==id);
            return obj;
        }

        public async Task<User> UpdateAsync(User obj)
        {
            return obj;
            
        }
    }
}

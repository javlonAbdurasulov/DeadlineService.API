﻿using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeadlineService.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DSDbContext _db;
        public UserRepository(DSDbContext db)
        {
            _db=db;
        }
        public async Task<User> CreateAsync(User obj)
        {
            obj.RoleId = 1;
            obj.PersonalInfo = new PersonalInfo();
            await _db.Users.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            User? obj= await GetByIdAsync(id);

            if (obj == null) return false;

            _db.Users.Remove(obj);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var allUsers = await _db.Users.ToListAsync();
            return allUsers;
        }
        public async Task<IEnumerable<User>> GetAllWithAllInformationAsync()
        {
            var allUsers = await _db.Users.Include(x => x.PersonalInfo).
                Include(x => x.Role).
                Include(x => x.AssignedOrders).
                Include(x => x.CreatedOrders).
                ToListAsync();
            return allUsers;
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user =await _db.Users.FirstOrDefaultAsync(x=>x.Username==username);
            return user;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            User? user = await _db.Users.
                Include(x => x.PersonalInfo).
                FirstOrDefaultAsync(x => x.PersonalInfo.Email == email);

            return user;
        }
        public async Task<User?> GetByIdAsync(int id)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(x=>x.Id==id);
            return user;
        }
        public async Task<User> UpdateAsync(User obj)
        {
            _db.Users.Update(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
    }
}

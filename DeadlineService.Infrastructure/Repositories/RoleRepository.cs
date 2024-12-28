using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Infrastructure.Repositories
{
    public class RoleRepository:IRoleRepository
    {
        private readonly DSDbContext _db;
        public RoleRepository(DSDbContext db)
        {
            _db = db;
        }

        public async Task<Role> CreateAsync(Role obj)
        {
        await    _db.Roles.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles=await _db.Roles.ToListAsync();    
            return roles;

        }

        public async Task<IEnumerable<Role>> GetAllWithUserAsync()
        {
            var roles = await _db.Roles.Include(x=>x.Users).ToListAsync();
            return roles;

        }

    }
}

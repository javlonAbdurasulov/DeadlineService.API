using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Infrastructure.Services
{
    public class PersonalInfoRepository : IPersonalInfoRepository
    {
        public readonly DSDbContext _db;

        public PersonalInfoRepository(DSDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<PersonalInfo> CreateAsync(PersonalInfo obj)
        {
            await _db.PersonalInfos.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            PersonalInfo personalInfo = await GetById(id);
            _db.PersonalInfos.Remove(personalInfo);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllAsync()
        {
            var result = await _db.PersonalInfos.ToListAsync();
            return result;
        }

        public async Task<PersonalInfo> GetById(int id)
        {
            var result = _db.PersonalInfos.FirstOrDefault(x => x.Id == id);
            return result;
        }

        public Task<PersonalInfo> UpdateAsync(PersonalInfo obj)
        {
            throw new NotImplementedException();
        }
    }
}

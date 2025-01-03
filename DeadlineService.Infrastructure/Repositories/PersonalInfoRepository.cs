﻿using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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
            PersonalInfo? personalInfo = await GetByIdAsync(id);
            _db.PersonalInfos.Remove(personalInfo);
            var result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<PersonalInfo>> GetAllAsync()
        {
            var result = await _db.PersonalInfos.ToListAsync();
            return result;
        }

        public async Task<PersonalInfo?> GetByIdAsync(int id)
        {
            PersonalInfo? result = await _db.PersonalInfos.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<PersonalInfo> UpdateAsync(PersonalInfo obj)
        {
            var trackedEntity = _db.PersonalInfos.Local.FirstOrDefault(e => e.Id == obj.Id);
            if (trackedEntity != null)
            {
                // Объект уже отслеживается, обновляем его напрямую
                _db.Entry(trackedEntity).CurrentValues.SetValues(obj);
            }
            else
            {
                // Объект не отслеживается, добавляем его
                _db.PersonalInfos.Update(obj);
            }
            await _db.SaveChangesAsync();
            return obj;
        }
    }
}

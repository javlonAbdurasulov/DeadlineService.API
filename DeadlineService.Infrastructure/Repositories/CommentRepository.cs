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
    public class CommentRepository : ICommentRepository
    {
        public readonly DSDbContext _db;

        public CommentRepository(DSDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<Comment> CreateAsync(Comment obj)
        {
            await _db.Comments.AddAsync(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Comment? comment = await GetByIdAsync(id);
            if (comment == null)
            {
                return false;
            }
            _db.Comments.Remove(comment);
            int result = await _db.SaveChangesAsync();
            return result > 0;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            var result = await _db.Comments.ToListAsync();
            return result;
        }

        public async Task<Comment?>GetByIdAsync(int id)
        {
            Comment? result =await _db.Comments.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Comment> UpdateAsync(Comment obj)
        {
            _db.Comments.Update(obj);
            await _db.SaveChangesAsync();
            return obj;
        }
    }
}

using DeadlineService.Application.Interfaces.Repostitories;
using DeadlineService.Domain.Models.Entity;
using DeadlineService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeadlineService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DSDbContext _db;
        public OrderRepository(DSDbContext db)
        {
            _db = db;
        }
        public async Task<Order> CreateAsync(Order obj)
        {
            using(_db)
            {
                _db.Orders.Add(obj);
                await _db.SaveChangesAsync();
            }
            return obj;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            int resultCountOfRemoved;
            using (_db)
            {
               Order order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == id);
               _db.Orders.Remove(order);
               resultCountOfRemoved= await _db.SaveChangesAsync();    
            }
            return resultCountOfRemoved > 0;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            IEnumerable<Order> allOrders = await _db.Orders.ToListAsync();
            return allOrders; 
        }

        public async Task<Order> UpdateAsync(Order obj)
        {
            using (_db)
            {
                _db.Orders.Update(obj);
                await _db.SaveChangesAsync();
            }
            return obj;
        }
    }
}

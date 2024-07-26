using DeadlineService.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Infrastructure.Data
{
    public class DSDbContext:DbContext
    {
        public DSDbContext()
        {

        }
        public DSDbContext(DbContextOptions<DSDbContext> dbContextOptions) : base(dbContextOptions) 
        { 
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.CreatedOrders)
            .WithOne(o => o.CreatedByUser)
            .HasForeignKey(o => o.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AssignedOrders)
                .WithOne(o => o.AssignedToUser)
                .HasForeignKey(o => o.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
        DbSet<User> Users { get; set; }
        DbSet<PersonalInfo> PersonalInfos{ get; set; }
        DbSet<Order> Orders{ get; set; }
        DbSet<Comment> Comments{ get; set; }
    }
}

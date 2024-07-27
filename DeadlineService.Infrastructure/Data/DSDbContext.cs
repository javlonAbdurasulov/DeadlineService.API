using DeadlineService.Domain.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DeadlineService.Infrastructure.Data
{
    public class DSDbContext : DbContext
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
        public DbSet<User> Users { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}

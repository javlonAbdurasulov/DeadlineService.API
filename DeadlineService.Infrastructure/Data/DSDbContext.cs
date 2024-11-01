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

            modelBuilder.Entity<User>()
            .HasOne(u => u.PersonalInfo)
            .WithOne(pi => pi.User)
            .HasForeignKey<PersonalInfo>(pi => pi.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Comment)        
                .WithOne(c => c.Order)         
                .HasForeignKey<Comment>(c => c.OrderId); 


            base.OnModelCreating(modelBuilder);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<PersonalInfo> PersonalInfos { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}

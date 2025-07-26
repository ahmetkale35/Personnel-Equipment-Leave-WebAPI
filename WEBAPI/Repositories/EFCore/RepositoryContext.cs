using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Config;
using System.Reflection;



namespace Repositories.EFCore
{
    public class RepositoryContext : IdentityDbContext<User>
    {
        public RepositoryContext(DbContextOptions options) :
            base(options) { }

        public DbSet<User> Users { get; set; }
        
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<EquipmentItem> EquipmentItems { get; set; }
        public DbSet<EquipmentRequests> EquipmentRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());   
            modelBuilder.Entity<EquipmentRequests>()
                .HasOne(e => e.Onaylayan)
                .WithMany()
                .HasForeignKey(e => e.OnaylayanId)
                .OnDelete(DeleteBehavior.Restrict); // yanlışlıkla kullanıcı silinince talep silinmesin
        }
    }
}

using AuditTrailAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace AuditTrailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.FirstName).HasMaxLength(100);
                e.Property(x => x.LastName).HasMaxLength(100);
                e.Property(x => x.Email).HasMaxLength(200);
            });

            b.Entity<AuditLog>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.EntityName).HasMaxLength(200).IsRequired();
                e.Property(x => x.EntityId).HasMaxLength(64).IsRequired();
                e.Property(x => x.ChangesJson).HasColumnType("nvarchar(max)");
                e.HasIndex(x => new { x.EntityName, x.EntityId, x.OccurredAt });
            });
        }

    }
}

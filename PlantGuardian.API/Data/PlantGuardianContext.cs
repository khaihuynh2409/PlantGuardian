using Microsoft.EntityFrameworkCore;
using PlantGuardian.API.Models;

namespace PlantGuardian.API.Data
{
    public class PlantGuardianContext : DbContext
    {
        public PlantGuardianContext(DbContextOptions<PlantGuardianContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<PlantLog> PlantLogs { get; set; }
        public DbSet<BeanDiaryEntry> BeanDiaryEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Plants)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Plant>()
                .HasMany(p => p.Logs)
                .WithOne(l => l.Plant)
                .HasForeignKey(l => l.PlantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BeanDiaryEntry>()
                .HasOne(e => e.Plant)
                .WithMany()
                .HasForeignKey(e => e.PlantId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

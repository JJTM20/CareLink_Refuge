using Microsoft.EntityFrameworkCore;
using CareLink_Refugee.Models;

namespace CareLink_Refugee.Persistence
{
    public class RefugeeDbContext : DbContext
    {
        public RefugeeDbContext(DbContextOptions<RefugeeDbContext> options) : base(options)
        {
        }

        public DbSet<Refugee> Refugees { get; set; }
        public DbSet<Shelter> Shelters { get; set; }
        public DbSet<Family> Families { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Refugee>()
            .HasOne(r => r.Accomodation)
            .WithMany(s => s.Refugees)
            .HasForeignKey(r => r.AccomodationId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Refugee>()
            .HasOne(r => r.Family)
            .WithMany(s => s.Members)
            .HasForeignKey(r => r.FamilyId)
            .OnDelete(DeleteBehavior.SetNull);

            base.OnModelCreating(modelBuilder);
        }
    }
}

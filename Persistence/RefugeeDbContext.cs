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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Refugee>()
            .HasOne(r => r.Accomodation)
            .WithMany(s => s.Refugees)
            .HasForeignKey(r => r.AccomodationId)
            .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}

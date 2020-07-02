using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace TranscripTrack.Data
{
    public class TrackerDbContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<LineRate> LineRates { get; set; }
        public DbSet<LineRateEntry> LineRateEntries { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        private static readonly string dbLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tracker.db");
        public TrackerDbContext()
        {
            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={dbLocation}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LineRateEntry>()
                .HasOne(lre => lre.LineRate)
                .WithMany(lr => lr.LineRateEntries)
                .HasForeignKey(lre => lre.LineRateId);
        }
    }
}

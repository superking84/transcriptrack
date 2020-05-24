﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    }
}

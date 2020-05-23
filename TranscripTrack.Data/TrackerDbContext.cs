using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace TranscripTrack.Data
{
    public class TrackerDbContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<LineRate> LineRates { get; set; }
        public DbSet<LineRateEntry> LineRateEntries { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tracker.db");
        }
    }

    public class Profile
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public List<LineRate> LineRates { get; } = new List<LineRate>();
    }

    public class LineRate
    {
        public int LineRateId { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public List<LineRateEntry> LineRateEntries { get; } = new List<LineRateEntry>();
    }

    public class LineRateEntry
    {
        public int LineRateEntryId { get; set; }
        public int NumLines { get; set; }
        public DateTime EnteredDate { get; set; }

        public int LineRateId { get; set; }
        public LineRate LineRate { get; set; }
    }

    public class Currency
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
    }
    //public enum Currency
    //{
    //    AUD,
    //    CAD,
    //    GBP,
    //    NZD,
    //    USD
    //}
}

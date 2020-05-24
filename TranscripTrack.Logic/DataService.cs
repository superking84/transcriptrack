using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.Logic
{
    public static class DataService
    {
        public static async Task<List<Currency>> GetCurrenciesAsync()
        {
            using (var db = new TrackerDbContext())
            {
                return await db.Currencies.ToListAsync();
            }
        }

        public static async Task AddProfileAsync(ProfileModel model)
        {
            using (var db = new TrackerDbContext())
            {
                var profile = new Profile
                {
                    Name = model.Name,
                    Client = model.Client,
                    CurrencyId = model.CurrencyId
                };

                await db.Profiles.AddAsync(profile);

                await db.SaveChangesAsync();
            }
        }

        public static async Task<List<ProfileSelectTableModel>> GetProfilesAsync()
        {
            using (var db = new TrackerDbContext())
            {
                return await (from p in db.Profiles
                              join c in db.Currencies on p.CurrencyId equals c.CurrencyId
                              select new ProfileSelectTableModel
                              {
                                  ProfileId = p.ProfileId,
                                  Name = p.Name,
                                  Client = p.Client,
                                  CurrencyCode = c.CurrencyCode
                              }).ToListAsync();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task<int> AddProfileAsync(ProfileModel model)
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

                return profile.ProfileId;
            }
        }

        public static async Task<bool> ProfileExistsAsync(int profileId)
        {
            using (var db = new TrackerDbContext())
            {
                return (await db.Profiles.FindAsync(profileId)) is Profile;
            }
        }

        public static async Task SaveLineEntryAsync(LineRateEntryEditModel model)
        {
            using (var db = new TrackerDbContext())
            {
                if (model.LineRateEntryId > 0)
                {
                    var existingModel = await db.LineRateEntries.FindAsync(model.LineRateEntryId);

                    existingModel.LineRateId = model.LineRateId;
                    existingModel.NumLines = model.NumLines;
                }
                else
                {
                    var newRecord = new LineRateEntry
                    {
                        LineRateId = model.LineRateId,
                        EnteredDate = model.EnteredDate,
                        NumLines = model.NumLines
                    };

                    await db.LineRateEntries.AddAsync(newRecord);
                }

                await db.SaveChangesAsync();
            }
        }

        public static async Task DeleteLineRateEntryAsync(int lineRateEntryId)
        {
            using (var db = new TrackerDbContext())
            {
                var existingRecord = await db.LineRateEntries.SingleAsync(lre => lre.LineRateEntryId == lineRateEntryId);

                db.LineRateEntries.Remove(existingRecord);

                await db.SaveChangesAsync();
            }
        }

        public static async Task<List<LineRate>> GetLineRatesAsync(int currentProfileId)
        {
            using (var db = new TrackerDbContext())
            {
                return await db.LineRates
                    .Where(lr => lr.ProfileId == currentProfileId)
                    .OrderBy(lr => lr.Description)
                    .ToListAsync();
            }
        }

        public static async Task<LineRateEntryEditModel> GetLineRateEntryAsync(int lineRateEntryId)
        {
            using (var db = new TrackerDbContext())
            {
                var lineRateEntry = await db.LineRateEntries.FindAsync(lineRateEntryId);

                return new LineRateEntryEditModel
                {
                    LineRateEntryId = lineRateEntry.LineRateEntryId,
                    LineRateId = lineRateEntry.LineRateId,
                    EnteredDate = lineRateEntry.EnteredDate,
                    NumLinesText = lineRateEntry.NumLines.ToString()
                };
            }
        }

        public static async Task<Profile> GetProfileAsync(int profileId)
        {
            using (var db = new TrackerDbContext())
            {
                return await db.Profiles.FindAsync(profileId);
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

        public static async Task<List<LineRateEntryTableModel>> GetLineRateEntriesAsync(DateTime lineEntryDate, int profileId)
        {
            using (var db = new TrackerDbContext())
            {
                return await (from lre in db.LineRateEntries
                              join lr in db.LineRates on lre.LineRateId equals lr.LineRateId
                              where lr.ProfileId == profileId
                              && lre.EnteredDate.Date == lineEntryDate.Date
                              select new LineRateEntryTableModel
                              {
                                  LineRateEntryId = lre.LineRateEntryId,
                                  LineRateType = lr.Description,
                                  NumLines = lre.NumLines,
                                  EnteredDate = lre.EnteredDate,
                                  AmountEarned = lre.NumLines * lr.Rate / 100
                              }).ToListAsync();
            }
        }
    }
}

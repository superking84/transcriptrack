using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.Logic
{
    public class LineRateEntryDataService : BaseDataService
    {
        public LineRateEntryDataService(TrackerDbContext db) : base(db)
        {
        }

        public async Task SaveAsync(LineRateEntryEditModel model)
        {
            if (model.LineRateEntryId != default)
            {
                var existingRecord = await db.LineRateEntries.FindAsync(model.LineRateEntryId);

                existingRecord.LineRateId = model.LineRateId;
                existingRecord.NumLines = model.NumLines;
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

        public async Task DeleteAsync(int id)
        {
            db.LineRateEntries.Remove(await db.LineRateEntries.FindAsync(id));

            await db.SaveChangesAsync();
        }

        public async Task<LineRateEntryEditModel> GetModelAsync(int id)
        {
            var lineRateEntry = await db.LineRateEntries.FindAsync(id);

            return new LineRateEntryEditModel
            {
                LineRateEntryId = lineRateEntry.LineRateEntryId,
                LineRateId = lineRateEntry.LineRateId,
                EnteredDate = lineRateEntry.EnteredDate,
                NumLinesText = lineRateEntry.NumLines.ToString()
            };
        }

        public async Task<List<LineRateEntryTableModel>> GetForProfileAndDateAsync(DateTime date, int profileId)
        {
            return await (from lre in db.LineRateEntries
                          join lr in db.LineRates on lre.LineRateId equals lr.LineRateId
                          where lr.ProfileId == profileId
                          && lre.EnteredDate.Date == date.Date
                          select new LineRateEntryTableModel
                          {
                              LineRateEntryId = lre.LineRateEntryId,
                              LineRateType = lr.Description,
                              NumLines = lre.NumLines,
                              EnteredDate = lre.EnteredDate,
                              AmountEarned = lre.NumLines * lr.Rate / 100
                          }).ToListAsync();
        }

        public async Task<LineRateEntryTotalModel> GetTotalsForDayAsync(DateTime date, int profileId)
        {
            var allLineRateEntries = await (from lre in db.LineRateEntries
                                            join lr in db.LineRates on lre.LineRateId equals lr.LineRateId
                                            where lr.ProfileId == profileId
                                            && lre.EnteredDate.Date == date.Date
                                            select new { lre.NumLines, AmountEarned = lre.NumLines * lr.Rate / 100 })
                                        .ToListAsync();

            return new LineRateEntryTotalModel
            {
                TotalLines = allLineRateEntries.Sum(lre => lre.NumLines),
                TotalPay = allLineRateEntries.Sum(lre => lre.AmountEarned)
            };
        }

        public async Task<LineRateEntryTotalModel> GetGrandTotalForMonthAsync(int month, int year, int profileId)
        {
            var allLineRateEntries = await GetForMonthAsync(month, year, profileId);

            return new LineRateEntryTotalModel
            {
                TotalLines = allLineRateEntries.Sum(lre => lre.TotalLines),
                TotalPay = allLineRateEntries.Sum(lre => lre.TotalPay)
            };
        }

        public async Task<List<LineRateEntryTotalModel>> GetTotalsByLineRateForMonthAsync(int month, int year, int profileId)
        {
            var allLineRateEntries = await GetForMonthAsync(month, year, profileId);

            return allLineRateEntries
                .GroupBy(lre => lre.LineRateId)
                .Select(grp => new LineRateEntryTotalModel
                {
                    LineRateId = grp.Key,
                    LineRate = grp.First().LineRate,
                    TotalLines = grp.Sum(lre => lre.TotalLines),
                    TotalPay = grp.Sum(lre => lre.TotalPay)
                })
                .ToList();
        }

        private async Task<List<LineRateEntryTotalModel>> GetForMonthAsync(int month, int year, int profileId)
        {
            return await (from lre in db.LineRateEntries
                          join lr in db.LineRates on lre.LineRateId equals lr.LineRateId
                          where lr.ProfileId == profileId
                          && lre.EnteredDate.Month == month
                          && lre.EnteredDate.Year == year
                          select new LineRateEntryTotalModel 
                          { 
                              LineRateId = lre.LineRateId,
                              LineRate = lr.Description,
                              TotalLines = lre.NumLines, 
                              TotalPay = lre.NumLines * lr.Rate / 100 
                          }).ToListAsync();
        }
    }
}

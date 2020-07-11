using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<LineRateEntryDailyTotalModel> GetTotalsForDayAsync(DateTime date, int profileId)
        {
            var allLineRateEntries = await (from lre in db.LineRateEntries
                                            join lr in db.LineRates on lre.LineRateId equals lr.LineRateId
                                            where lr.ProfileId == profileId
                                            && lre.EnteredDate.Date == date.Date
                                            select new { lre.NumLines, AmountEarned = lre.NumLines * lr.Rate / 100 })
                                        .ToListAsync();

            return new LineRateEntryDailyTotalModel
            {
                TotalLines = allLineRateEntries.Sum(lre => lre.NumLines),
                TotalPay = allLineRateEntries.Sum(lre => lre.AmountEarned)
            };
        }
    }
}

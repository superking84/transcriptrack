﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.Logic
{
    public class LineRateDataService : BaseDataService
    {
        public LineRateDataService(TrackerDbContext db) : base(db)
        {
        }

        public async Task SaveAsync(LineRateEditModel model)
        {
            if (model.LineRateId != default)
            {
                var existingRecord = await db.LineRates.FindAsync(model.LineRateId);

                existingRecord.Description = model.Description;
                existingRecord.Rate = model.Rate;
            }
            else
            {
                var newRecord = new LineRate
                {
                    ProfileId = model.ProfileId,
                    Description = model.Description,
                    Rate = model.Rate
                };

                await db.LineRates.AddAsync(newRecord);
            }

            await db.SaveChangesAsync();
        }

        public async Task<List<LineRateEditModel>> GetAllForEditAsync(int profileId)
        {
            return (await db.LineRates
                .Where(lr => lr.ProfileId == profileId)
                .OrderBy(lr => lr.Description)
                .ToListAsync())
                .Select(lr => new LineRateEditModel
                {
                    LineRateId = lr.LineRateId,
                    ProfileId = lr.ProfileId,
                    Description = lr.Description,
                    RateText = lr.Rate.ToString()
                })
                .ToList();
        }

        public async Task SaveChangesAsync(List<LineRateEditModel> lineRates, int profileId)
        {
            var newRecords = lineRates
                .Where(lr => lr.LineRateId == default)
                .Select(lr => new LineRate
                {
                    ProfileId = profileId,
                    Description = lr.Description,
                    Rate = lr.Rate
                });
            await db.LineRates.AddRangeAsync(newRecords);

            var existingLineRates = lineRates
                .Where(lr => lr.LineRateId != default);
            foreach (var existing in existingLineRates)
            {
                var recordToUpdate = await db.LineRates.FindAsync(existing.LineRateId);

                recordToUpdate.Description = existing.Description;
                recordToUpdate.Rate = existing.Rate;
            }
            
            var existingLineRateIds = existingLineRates.Select(lr => lr.LineRateId);
            var recordsToDelete = db.LineRates.Where(lr => lr.ProfileId == profileId && !existingLineRateIds.Contains(lr.LineRateId));
            
            db.LineRates.RemoveRange(recordsToDelete);

            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            db.LineRates.Remove(await db.LineRates.FindAsync(id));

            await db.SaveChangesAsync();
        }

        public async Task<List<LineRate>> GetForProfileAsync(int profileId)
        {
            return await db.LineRates
                    .Where(lr => lr.ProfileId == profileId)
                    .OrderBy(lr => lr.Description)
                    .ToListAsync();
        }

        public async Task<LineRateEditModel> GetModelAsync(int id)
        {
            var lineRate = await db.LineRates.FindAsync(id);

            return new LineRateEditModel
            {
                LineRateId = lineRate.LineRateId,
                ProfileId = lineRate.ProfileId,
                Description = lineRate.Description,
                RateText = lineRate.Rate.ToString()
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.Logic
{
    public sealed class ProfileDataService : BaseDataService
    {
        public ProfileDataService(TrackerDbContext dbContext) : base(dbContext)
        {
        }

        private async Task<int> AddAsync(Profile profile)
        {
            await db.Profiles.AddAsync(profile);

            await db.SaveChangesAsync();
            
            return profile.ProfileId;
        }

        public async Task<int> SaveAsync(ProfileModel model)
        {
            Profile profile;
            if (model.ProfileId.HasValue)
            {
                profile = await GetAsync(model.ProfileId.Value);
                
                return await EditAsync(profile);
            }
            else
            {
                profile = new Profile
                {
                    Name = model.Name,
                    Client = model.Client,
                    CurrencyId = model.CurrencyId
                };

                return await AddAsync(profile);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingRecord = await GetAsync(id);
            db.Profiles.Remove(existingRecord);

            await db.SaveChangesAsync();
        }

        private async Task<int> EditAsync(Profile profile)
        {
            db.Profiles.Update(profile);
            
            await db.SaveChangesAsync();

            return profile.ProfileId;
        }

        public async Task<Profile> GetAsync(int id)
        {
            return await db.Profiles.FindAsync(id);
        }
    }
}

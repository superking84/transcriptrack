using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.Logic
{
    public sealed class ProfileDataService : BaseDataService
    {
        public ProfileDataService(TrackerDbContext db) : base(db)
        {
        }

        private async Task<int> AddAsync(Profile profile)
        {
            await db.Profiles.AddAsync(profile);

            await db.SaveChangesAsync();

            return profile.ProfileId;
        }

        public async Task<int> SaveAsync(ProfileEditModel model)
        {
            Profile profile;
            if (model.ProfileId.HasValue)
            {
                profile = await db.Profiles.FindAsync(model.ProfileId.Value);
                profile.Name = model.Name;
                profile.Client = model.Client;
                profile.CurrencyId = model.CurrencyId;

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
            // TO DO: Cascade deletion down to Line Rates and Line Rate Entries, or is it automatic?  Need to confirm
            var existingRecord = await db.Profiles.FindAsync(id);
            db.Profiles.Remove(existingRecord);

            await db.SaveChangesAsync();
        }

        private async Task<int> EditAsync(Profile profile)
        {
            db.Profiles.Update(profile);

            await db.SaveChangesAsync();

            return profile.ProfileId;
        }

        public async Task<ProfileEditModel> GetModelAsync(int id)
        {
            if (await db.Profiles.FindAsync(id) is Profile existingProfile)
            {
                return new ProfileEditModel
                {
                    ProfileId = existingProfile.ProfileId,
                    Name = existingProfile.Name,
                    Client = existingProfile.Client,
                    CurrencyId = existingProfile.CurrencyId
                };
            }

            return null;
        }

        public async Task<List<ProfileSelectTableModel>> GetSelectProfileListAsync()
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

        public async Task<bool> ExistsAsync(int id)
        {
            return (await db.Profiles.FindAsync(id)) is Profile;
        }
    }
}

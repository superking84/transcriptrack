using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TranscripTrack.Data;

namespace TranscripTrack.Logic
{
    public class LineRateDataService : BaseDataService
    {
        public LineRateDataService(TrackerDbContext db) : base(db)
        {
        }

        public async Task<List<LineRate>> GetForProfileAsync(int profileId)
        {
            return await db.LineRates
                    .Where(lr => lr.ProfileId == profileId)
                    .OrderBy(lr => lr.Description)
                    .ToListAsync();
        }


    }
}

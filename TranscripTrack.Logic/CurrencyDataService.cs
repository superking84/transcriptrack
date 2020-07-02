using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TranscripTrack.Data;

namespace TranscripTrack.Logic
{
    public class CurrencyDataService : BaseDataService
    {
        public CurrencyDataService(TrackerDbContext db) : base(db)
        {
        }

        public async Task<List<Currency>> GetAllAsync()
        {
            return await db.Currencies.ToListAsync();
        }
    }
}

using System.Threading.Tasks;
using TranscripTrack.Data;

namespace TranscripTrack.Logic
{
    public abstract class BaseDataService
    {
        protected TrackerDbContext db;
        public BaseDataService(TrackerDbContext db)
        {
            this.db = db;
        }
    }
}

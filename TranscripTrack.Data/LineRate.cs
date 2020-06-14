using System.Collections.Generic;

namespace TranscripTrack.Data
{
    public class LineRate
    {
        public int LineRateId { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }

        public int ProfileId { get; set; }
        public Profile Profile { get; set; }

        public List<LineRateEntry> LineRateEntries { get; set; }
    }
}

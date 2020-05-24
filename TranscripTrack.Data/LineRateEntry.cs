using System;

namespace TranscripTrack.Data
{
    public class LineRateEntry
    {
        public int LineRateEntryId { get; set; }
        public int NumLines { get; set; }
        public DateTime EnteredDate { get; set; }

        public int LineRateId { get; set; }
        public LineRate LineRate { get; set; }
    }
}

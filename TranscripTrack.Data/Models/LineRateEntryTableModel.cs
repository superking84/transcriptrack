using System;

namespace TranscripTrack.Data.Models
{
    public class LineRateEntryTableModel : BaseModel
    {
        public int LineRateEntryId { get; set; }
        public string LineRateType { get; set; }
        public int NumLines { get; set; }
        public DateTime EnteredDate { get; set; }
        public decimal AmountEarned { get; set; }
        public bool HighlightRow { get; set; }
    }
}

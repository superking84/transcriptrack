using System;
using System.Collections.Generic;
using System.Text;

namespace TranscripTrack.Data.Models
{
    public class LineRateEntryDailyTotalModel : BaseModel
    {
        public int TotalLines { get; set; }
        public decimal TotalPay { get; set; }
    }
}

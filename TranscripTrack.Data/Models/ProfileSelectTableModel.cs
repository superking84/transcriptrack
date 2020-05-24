using System;
using System.Collections.Generic;
using System.Text;

namespace TranscripTrack.Data.Models
{
    public class ProfileSelectTableModel : BaseModel
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public string CurrencyCode { get; set; }
    }
}

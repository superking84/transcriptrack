using System.Collections.Generic;

namespace TranscripTrack.Data
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }

        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public List<LineRate> LineRates { get; } = new List<LineRate>();
    }
}

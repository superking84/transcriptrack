using System.ComponentModel.DataAnnotations.Schema;

namespace TranscripTrack.Data
{
    [Table("Currencies")]
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
    }
}

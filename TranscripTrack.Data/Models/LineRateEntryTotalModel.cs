namespace TranscripTrack.Data.Models
{
    public class LineRateEntryTotalModel : BaseModel
    {
        public int? LineRateId { get; set; }
        public string LineRate { get; set; }

        public int TotalLines { get; set; }
        public decimal TotalPay { get; set; }
    }
}

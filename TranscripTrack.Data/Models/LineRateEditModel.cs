using System;
using System.Collections.Generic;
using System.Text;

namespace TranscripTrack.Data.Models
{
    public class LineRateEditModel : BaseModel
    {
        public int LineRateId { get; set; }
        public int ProfileId { get; set; }
        public string Description { get; set; }

        private string rateText;
        public string RateText {
            get => rateText;
            set {
                if (string.IsNullOrEmpty(value) || decimal.TryParse(value, out decimal _))
                {
                    rateText = value;
                    OnPropertyChanged("RateText");
                }
            }
        }

        public decimal Rate => decimal.Parse(rateText);
    }
}

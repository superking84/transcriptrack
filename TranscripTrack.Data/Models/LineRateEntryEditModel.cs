using System;
using System.Collections.Generic;
using System.Text;

namespace TranscripTrack.Data.Models
{
    public class LineRateEntryEditModel : BaseModel
    {
        public int LineRateEntryId { get; set; }
        public DateTime EnteredDate { get; set; }

        
        private string numLinesText;
        public string NumLinesText {
            get => numLinesText;
            set {
                if (string.IsNullOrEmpty(value) || int.TryParse(value, out int _)){
                    numLinesText = value;
                    OnPropertyChanged("NumLinesText");
                }
            }
        }
        public int NumLines => int.Parse(NumLinesText);

        private int lineRateId;
        public int LineRateId {
            get => lineRateId;
            set {
                lineRateId = value;
                OnPropertyChanged("LineRateId");
            }
        }
    }
}

namespace TranscripTrack.Data.Models
{
    public class ProfileEditModel : BaseModel
    {
        private string name;
        private int currencyId;

        public int? ProfileId { get; set; }
        public string Name {
            get => name;
            set {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int CurrencyId {
            get => currencyId;
            set {
                currencyId = value;
                OnPropertyChanged(nameof(CurrencyId));
            }
        }
    }
}

﻿namespace TranscripTrack.Data.Models
{
    public class ProfileEditModel : BaseModel
    {
        private string name;
        private string client;

        private int currencyId;

        public int? ProfileId { get; set; }
        public string Name {
            get => name;
            set {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Client {
            get => client;
            set {
                client = value;
                OnPropertyChanged("Client");
            }
        }

        public int CurrencyId {
            get => currencyId;
            set {
                currencyId = value;
                OnPropertyChanged("CurrencyId");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Logic;

namespace TranscripTrack.UI.ViewModels
{
    public class EntryViewModel : BaseViewModel
    {
        public int ProfileId { get; set; }
        //public string Title { get; set; }

        private Profile profile;
        public Profile Profile {
            get => profile;
            set {
                profile = value;
                OnPropertyChanged("Profile");
            }
        }

        private List<LineRate> lineRates;
        public List<LineRate> LineRates {
            get => lineRates;
            set {
                lineRates = value;
                OnPropertyChanged("LineRates");
            }
        }

        private List<LineRateEntry> lineRateEntries;
        public List<LineRateEntry> LineRateEntries {
            get => lineRateEntries;
            set {
                lineRateEntries = value;
                OnPropertyChanged("LineRateEntries");
            }
        }


        public async Task InitializeAsync()
        {
            Profile = await DataService.GetProfileAsync(ProfileId);

            Title = $"TranscripTrack - {Profile.Name} ({Profile.Client})";
        }
    }
}

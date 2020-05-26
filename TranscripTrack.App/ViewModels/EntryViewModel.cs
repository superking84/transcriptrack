using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class EntryViewModel : BaseViewModel
    {
        private int profileId;
        public int ProfileId {
            get => profileId;
            set {
                profileId = value;
                OnPropertyChanged("ProfileId");
            }
        }

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

        protected override async void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == "ProfileId")
            {
                await LoadProfileAsync();
            }
        }

        private async Task LoadProfileAsync()
        {
            Profile = await DataService.GetProfileAsync(ProfileId);

            Title = $"TranscripTrack - {Profile.Name} ({Profile.Client})";
        }

        public async Task InitializeAsync()
        {
            await LoadProfileAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.UI.ViewModels
{
    public class SelectProfileViewModel : BaseViewModel
    {
        private List<ProfileSelectTableModel> profiles;
        public List<ProfileSelectTableModel> Profiles {
            get => profiles;
            set {
                profiles = value;

                OnPropertyChanged("Profiles");
            }
        }

        public async Task InitializeAsync()
        {
            Profiles = await DataService.GetProfilesAsync();
        }
    }
}

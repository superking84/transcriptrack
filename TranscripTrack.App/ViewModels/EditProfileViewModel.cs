using System.Collections.Generic;
using System.Threading.Tasks;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private List<Currency> currencies;
        public List<Currency> Currencies {
            get => currencies;
            set {
                currencies = value;
                OnPropertyChanged("Currencies");
            }
        }


        public ProfileModel Model { get; private set; }
        //public string Title { get; private set; }

        public EditProfileViewModel(bool isAdd)
        {
            Title = isAdd ? "Add New Profile" : "Edit Profile";

            Model = new ProfileModel();
        }

        public async Task InitializeAsync()
        {
            Currencies = await DataService.GetCurrenciesAsync();
        }

        public async Task AddProfileAsync()
        {
            await DataService.AddProfileAsync(Model);
        }
    }
}

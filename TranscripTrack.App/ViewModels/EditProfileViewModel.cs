using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private readonly bool isAdd;
        public RelayCommand<Window> SaveCommand { get; set; }

        private List<Currency> currencies;
        public List<Currency> Currencies {
            get => currencies;
            set {
                currencies = value;
                OnPropertyChanged("Currencies");
            }
        }

        public ProfileModel Model { get; private set; }
        
        public EditProfileViewModel(int? profileId)
        {
            isAdd = !profileId.HasValue;

            Title = isAdd ? "Add New Profile" : "Edit Profile";

            if (isAdd)
            {
                SaveCommand = new RelayCommand<Window>(SaveNewProfileAsync, CanSaveNewProfile);
            }
            else
            {
                //??????
            }
            Model = new ProfileModel();
        }

        private async void SaveNewProfileAsync(Window window)
        {
            var profileId = await DataService.AddProfileAsync(Model);
            
            Properties.UserSettings.Default.CurrentProfileId = profileId;
            Properties.UserSettings.Default.Save();

            window?.Close();
        }

        // will implement cleaner validation later
        private bool CanSaveNewProfile(Window window)
        {
            return !string.IsNullOrWhiteSpace(Model.Name) &&
                !string.IsNullOrWhiteSpace(Model.Client) &&
                Model.CurrencyId != default;
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            Currencies = await DataService.GetCurrenciesAsync();
        }
    }
}

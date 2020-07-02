using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private readonly int? profileId;
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

        public ProfileEditModel Model { get; private set; }

        public EditProfileViewModel(int? profileId)
        {
            this.profileId = profileId;
            isAdd = !profileId.HasValue;

            Title = isAdd ? "Add New Profile" : "Edit Profile";

            SaveCommand = new RelayCommand<Window>(SaveProfileAsync, CanSaveProfile);

            Model = new ProfileEditModel();
        }   

        private async void SaveProfileAsync(Window window)
        {
            var profileId = await App.ProfileDataService.SaveAsync(Model);

            Properties.UserSettings.Default.CurrentProfileId = profileId;
            Properties.UserSettings.Default.Save();

            window?.Close();
        }

        // will implement cleaner validation later
        private bool CanSaveProfile(Window window)
        {
            return !string.IsNullOrWhiteSpace(Model.Name) &&
                !string.IsNullOrWhiteSpace(Model.Client) &&
                Model.CurrencyId != default;
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            Currencies = await App.CurrencyDataService.GetAllAsync();

            if (profileId.HasValue)
            {
                var profileModel = await App.ProfileDataService.GetModelAsync(profileId.Value);
                Model.ProfileId = profileId.Value;
                Model.Name = profileModel.Name;
                Model.Client = profileModel.Client;
                Model.CurrencyId = profileModel.CurrencyId;
            }
        }
    }
}

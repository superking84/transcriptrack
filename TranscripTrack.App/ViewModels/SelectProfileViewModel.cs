using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class SelectProfileViewModel : BaseViewModel
    {
        public RelayCommand<Window> ItemSelectCommand { get; private set; }

        public SelectProfileViewModel()
        {
            Title = "Select A Profile";

            ItemSelectCommand = new RelayCommand<Window>(SelectProfile, CanSelectProfile);
        }

        private bool CanSelectProfile(Window window)
        {
            return SelectedProfile != null;
        }

        private void SelectProfile(Window window)
        {
            Properties.UserSettings.Default.CurrentProfileId = SelectedProfile.ProfileId;
            Properties.UserSettings.Default.Save();

            window?.Close();
        }

        private List<ProfileSelectTableModel> profiles;
        public List<ProfileSelectTableModel> Profiles {
            get => profiles;
            set {
                profiles = value;

                OnPropertyChanged(nameof(Profiles));
            }
        }
        public ProfileSelectTableModel SelectedProfile { get; set; }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            Profiles = await App.ProfileDataService.GetSelectProfileListAsync(Properties.UserSettings.Default.CurrentProfileId);
        }
    }
}

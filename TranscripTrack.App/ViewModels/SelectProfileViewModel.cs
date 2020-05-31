using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class SelectProfileViewModel : BaseViewModel
    {
        public RelayCommand InitializeCommand { get; private set; }
        public RelayCommand<Window> ItemSelectCommand { get; set; }

        public SelectProfileViewModel()
        {
            InitializeCommand = new RelayCommand(InitializeAsync);
            ItemSelectCommand = new RelayCommand<Window>(SelectProfileAsync, CanSelectProfile);
        }

        private bool CanSelectProfile(Window window)
        {
            return SelectedProfile != null;
        }

        private void SelectProfileAsync(Window window)
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
                
                OnPropertyChanged("Profiles");
            }
        }
        public ProfileSelectTableModel SelectedProfile { get; set; }

        public async void InitializeAsync()
        {
            Profiles = await DataService.GetProfilesAsync();
        }
    }
}

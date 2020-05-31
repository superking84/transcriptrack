using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TranscripTrack.App.Views;
using TranscripTrack.Data;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand SelectProfileCommand { get; set; }
        public ICommand AddProfileCommand { get; set; }
        public ICommand InitializeCommand { get; set; }

        public MainViewModel()
        {
            SelectProfileCommand = new RelayCommand(OpenSelectProfileModal);
            AddProfileCommand = new RelayCommand(OpenAddProfileModal);
            InitializeCommand = new RelayCommand(InitializeAsync);
        }

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

        public async Task LoadProfileAsync()
        {
            ProfileId = Properties.UserSettings.Default.CurrentProfileId;
            Profile = await DataService.GetProfileAsync(ProfileId);

            Application.Current.MainWindow.Title = $"TranscripTrack - {Profile.Name} ({Profile.Client})";
        }

        public async void InitializeAsync()
        {
            await LoadProfileAsync();
        }

        private void OpenSelectProfileModal()
        {
            var selectProfileView = new SelectProfileView();
            selectProfileView.CloseEvent += new EventHandler(async (sender, e) => await LoadProfileAsync());
            selectProfileView.ShowDialog();
        }

        private void OpenAddProfileModal()
        {
            (new EditProfileView()).ShowDialog();
        }

    }
}

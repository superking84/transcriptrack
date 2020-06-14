using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TranscripTrack.App.Views;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private EditLineRateEntryView editLineRateEntryView;
        private EditProfileView editProfileView;
        private SelectProfileView selectProfileView;

        public RelayCommand SelectProfileCommand { get; private set; }
        public RelayCommand AddProfileCommand { get; private set; }
        public RelayCommand AddLineRateEntryCommand { get; private set; }

        public MainViewModel()
        {
            SelectProfileCommand = new RelayCommand(OpenSelectProfileModal);
            AddProfileCommand = new RelayCommand(OpenAddProfileModal);
            AddLineRateEntryCommand = new RelayCommand(OpenAddLineEntryModal);
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

        private List<LineRateEntryTableModel> lineRateEntries;
        public List<LineRateEntryTableModel> LineRateEntries {
            get => lineRateEntries;
            set {
                lineRateEntries = value;
                OnPropertyChanged("LineRateEntries");
            }
        }

        private DateTime lineEntryDate;
        public DateTime LineEntryDate {
            get => lineEntryDate;
            set {
                lineEntryDate = value;
                OnPropertyChanged("LineEntryDate");
            }
        }

        public async Task LoadCurrentProfileAsync()
        {
            ProfileId = Properties.UserSettings.Default.CurrentProfileId;
            Profile = await DataService.GetProfileAsync(ProfileId);

            Application.Current.MainWindow.Title = $"TranscripTrack - {Profile.Name} ({Profile.Client})";
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            await LoadCurrentProfileAsync();

            LineEntryDate = DateTime.Today;
        }

        public async void OnEntryDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await LoadLineRateEntriesAsync();
        }

        private async Task LoadLineRateEntriesAsync()
        {
            LineRateEntries = await DataService.GetLineRateEntriesAsync(LineEntryDate, ProfileId);
        }

        private void OpenSelectProfileModal()
        {
            selectProfileView = new SelectProfileView();
            selectProfileView.Closed += new EventHandler(OnSelectProfileClosed);
            selectProfileView.ShowDialog();

            GC.Collect();
        }

        private async void OnSelectProfileClosed(object sender, EventArgs e)
        {
            if (ProfileId != Properties.UserSettings.Default.CurrentProfileId)
            {
                await LoadCurrentProfileAsync();
            }
        }

        private async void OnAddProfileClosed(object sender, EventArgs e)
        {
            if (ProfileId != Properties.UserSettings.Default.CurrentProfileId)
            {
                await LoadCurrentProfileAsync();
            }
        }

        private async void OnEditLineRateEntryClosed(object sender, EventArgs e)
        {
            await LoadLineRateEntriesAsync();
        }

        private void OpenAddProfileModal()
        {
            editProfileView = new EditProfileView(null);
            editProfileView.Closed += new EventHandler(OnAddProfileClosed);
            editProfileView.ShowDialog();

            GC.Collect();
        }

        private void OpenAddLineEntryModal()
        {
            editLineRateEntryView = new EditLineRateEntryView(LineEntryDate, null);
            editLineRateEntryView.Closed += new EventHandler(OnEditLineRateEntryClosed);
            editLineRateEntryView.ShowDialog();

            GC.Collect();
        }
    }
}

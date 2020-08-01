using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TranscripTrack.App.Views;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private EditLineRateEntryView editLineRateEntryView;
        private EditProfileView editProfileView;
        private SelectProfileView selectProfileView;
        private ManageLineRatesView manageLineRatesView;
        private LineRateEntryTotalsView lineRateEntryTotalsView;

        private readonly EventHandler editLineRateEntryClosedHandler;

        public RelayCommand SelectProfileCommand { get; private set; }
        public RelayCommand AddProfileCommand { get; private set; }
        public RelayCommand EditProfileCommand { get; private set; }
        public RelayCommand DeleteProfileCommand { get; private set; }
        public RelayCommand ManageLineRatesCommand { get; private set; }
        public RelayCommand ViewLineRateEntryTotalsCommand { get; private set; }
        public RelayCommand AddLineRateEntryCommand { get; private set; }
        public RelayCommand EditLineRateEntryCommand { get; private set; }
        public RelayCommand<LineRateEntryTableModel> EditLineRateEntryInLineCommand { get; private set; }
        public RelayCommand DeleteLineRateEntryCommand { get; private set; }
        public RelayCommand<LineRateEntryTableModel> DeleteLineRateEntryInLineCommand { get; private set; }
        public RelayCommand ExitApplicationCommand { get; private set; }

        public MainViewModel()
        {
            SelectProfileCommand = new RelayCommand(OpenSelectProfileModal, CanSelectProfile);
            AddProfileCommand = new RelayCommand(OpenAddProfileModal);
            EditProfileCommand = new RelayCommand(OpenEditProfileModal, IsProfileLoaded);
            DeleteProfileCommand = new RelayCommand(ConfirmDeleteProfile, IsProfileLoaded);
            ManageLineRatesCommand = new RelayCommand(OpenManageLineRatesModal, IsProfileLoaded);
            ViewLineRateEntryTotalsCommand = new RelayCommand(OpenLineRateEntryTotalsModal, IsProfileLoaded);
            AddLineRateEntryCommand = new RelayCommand(OpenAddLineEntryModal, CanAddLineRateEntry);
            EditLineRateEntryCommand = new RelayCommand(OpenEditLineRateEntryModal, CanUpdateLineRateEntry);
            EditLineRateEntryInLineCommand = new RelayCommand<LineRateEntryTableModel>(OpenEditLineRateEntryInLineModal);
            DeleteLineRateEntryCommand = new RelayCommand(ConfirmDeleteLineRateEntry, CanUpdateLineRateEntry);
            DeleteLineRateEntryInLineCommand = new RelayCommand<LineRateEntryTableModel>(ConfirmDeleteLineRateEntryInLine);
            ExitApplicationCommand = new RelayCommand(ExitApplication);

            editLineRateEntryClosedHandler = new EventHandler(OnEditLineRateEntryClosed);

            DailyTotals = new List<LineRateEntryTotalModel>();
        }

        private void ExitApplication()
        {
            App.Current.Shutdown();
        }

        private void OpenLineRateEntryTotalsModal()
        {
            lineRateEntryTotalsView = new LineRateEntryTotalsView(LineEntryDate.Month, LineEntryDate.Year);
            lineRateEntryTotalsView.ShowDialog();

            GC.Collect();
        }

        private void OpenManageLineRatesModal()
        {
            manageLineRatesView = new ManageLineRatesView();
            manageLineRatesView.Closed += new EventHandler(OnManageLineRatesClosed);
            manageLineRatesView.ShowDialog();

            GC.Collect();
        }

        private async void ConfirmDeleteProfile()
        {
            var response = MessageBox.Show(
                "Delete this profile?  This action cannot be undone, and all associated data will be permanently lost.",
                "Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (response == MessageBoxResult.OK)
            {
                await App.ProfileDataService.DeleteAsync(ProfileId);

                Properties.UserSettings.Default.CurrentProfileId = default;
                Properties.UserSettings.Default.Save();

                await LoadCurrentProfileAsync();
            }
        }

        private bool CanSelectProfile()
        {
            return OtherProfileCount >= 1;
        }

        private bool CanAddLineRateEntry()
        {
            return IsProfileLoaded() && (LineRates?.Any() == true);
        }

        private bool IsProfileLoaded()
        {
            return Profile is ProfileEditModel;
        }

        private async void ConfirmDeleteLineRateEntry()
        {
            await ConfirmDeleteLineRateEntry(SelectedLineRateEntry.LineRateEntryId);
        }

        private async Task ConfirmDeleteLineRateEntry(int lineRateEntryId)
        {
            var response = MessageBox.Show(
                "Delete this record?  This action cannot be undone.",
                "Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (response == MessageBoxResult.OK)
            {
                await App.LineRateEntryDataService.DeleteAsync(lineRateEntryId);
                await LoadLineRateEntriesAsync();
            }
        }
        private async void ConfirmDeleteLineRateEntryInLine(LineRateEntryTableModel entry)
        {
            await ConfirmDeleteLineRateEntry(entry.LineRateEntryId);
        }

        private int profileId;
        public int ProfileId {
            get => profileId;
            set {
                profileId = value;
                OnPropertyChanged("ProfileId");
            }
        }

        private ProfileEditModel profile;
        public ProfileEditModel Profile {
            get => profile;
            set {
                profile = value;
                OnPropertyChanged("Profile");
            }
        }

        private int otherProfileCount;
        public int OtherProfileCount {
            get => otherProfileCount;
            set {
                otherProfileCount = value;
                OnPropertyChanged("OtherProfileCount");
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

                foreach (var entry in lineRateEntries)
                {
                    entry.HighlightRow = lineRateEntries.IndexOf(entry) % 2 == 1;
                }
            }
        }

        private LineRateEntryTableModel selectedLineRateEntry;
        public LineRateEntryTableModel SelectedLineRateEntry {
            get => selectedLineRateEntry;
            set {
                selectedLineRateEntry = value;
                OnPropertyChanged("SelectedLineRateEntry");
            }
        }
        public bool CanDeleteEntry => SelectedLineRateEntry is LineRateEntryTableModel;

        private DateTime lineEntryDate;
        public DateTime LineEntryDate {
            get => lineEntryDate;
            set {
                lineEntryDate = value;
                OnPropertyChanged("LineEntryDate");
            }
        }

        private List<LineRateEntryTotalModel> dailyTotals;
        public List<LineRateEntryTotalModel> DailyTotals {
            get => dailyTotals;
            set {
                dailyTotals = value;
                OnPropertyChanged("DailyTotals");
            }
        }


        public async Task LoadCurrentProfileAsync()
        {
            ProfileId = Properties.UserSettings.Default.CurrentProfileId;
            Profile = await App.ProfileDataService.GetModelAsync(ProfileId);
            OtherProfileCount = await App.ProfileDataService.GetCountAsync(ProfileId);

            if (Profile is ProfileEditModel)
            {
                Application.Current.MainWindow.Title = $"TranscripTrack - {Profile.Name}";
                
                LineRates = await App.LineRateDataService.GetForProfileAsync(ProfileId);
                if (!LineRates.Any())
                {
                    // if no line rates exist, prompt the user to create one or more
                    OpenManageLineRatesModal();
                }
                else
                {
                    // there's no need to try to load LREs unless LRs exist
                    await LoadLineRateEntriesAsync();
                }
            }
            else
            {
                Application.Current.MainWindow.Title = "TranscripTrack";
            }

        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            LineEntryDate = DateTime.Today;

            if (Properties.UserSettings.Default.CurrentProfileId == default)
            {
                if (await App.ProfileDataService.ExistAnyAsync())
                {
                    OpenSelectProfileModal();
                }
                else
                {
                    OpenAddProfileModal();
                }
            }
            else
            {
                await LoadCurrentProfileAsync();
            }
        }

        public async void OnManageLineRatesClosed(object sender, EventArgs e)
        {
            await LoadCurrentProfileAsync();
        }

        public async void OnEntryDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await LoadLineRateEntriesAsync();
        }

        private async Task LoadLineRateEntriesAsync()
        {
            LineRateEntries = await App.LineRateEntryDataService.GetForProfileAndDateAsync(LineEntryDate, ProfileId);
            DailyTotals = new List<LineRateEntryTotalModel>()
            {
                await App.LineRateEntryDataService.GetTotalsForDayAsync(LineEntryDate, ProfileId)
            };
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

        private void OpenEditProfileModal()
        {
            editProfileView = new EditProfileView(ProfileId);
            editProfileView.Closed += new EventHandler(OnEditProfileClosed);
            editProfileView.ShowDialog();

            GC.Collect();
        }

        private async void OnEditProfileClosed(object sender, EventArgs e)
        {
            await LoadCurrentProfileAsync();
        }

        private void OpenAddLineEntryModal()
        {
            editLineRateEntryView = new EditLineRateEntryView(LineEntryDate, null);
            editLineRateEntryView.Closed += editLineRateEntryClosedHandler;
            editLineRateEntryView.ShowDialog();

            GC.Collect();
        }

        private void OpenEditLineRateEntryModal()
        {
            OpenEditLineRateEntryModal(SelectedLineRateEntry.EnteredDate, SelectedLineRateEntry.LineRateEntryId);
        }

        private void OpenEditLineRateEntryModal(DateTime date, int lineRateEntryId)
        {
            editLineRateEntryView = new EditLineRateEntryView(date, lineRateEntryId);
            editLineRateEntryView.Closed += editLineRateEntryClosedHandler;
            editLineRateEntryView.ShowDialog();

            GC.Collect();
        }
        private void OpenEditLineRateEntryInLineModal(LineRateEntryTableModel entry)
        {
            OpenEditLineRateEntryModal(entry.EnteredDate, entry.LineRateEntryId);
        }

        private bool CanUpdateLineRateEntry()
        {
            return SelectedLineRateEntry is LineRateEntryTableModel;
        }
    }
}

﻿using GalaSoft.MvvmLight.CommandWpf;
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

        private readonly EventHandler editLineRateEntryClosedHandler;

        public RelayCommand SelectProfileCommand { get; private set; }
        public RelayCommand AddProfileCommand { get; private set; }
        public RelayCommand EditProfileCommand { get; private set; }
        public RelayCommand DeleteProfileCommand { get; private set; }
        public RelayCommand ManageLineRatesCommand { get; private set; }
        public RelayCommand AddLineRateEntryCommand { get; private set; }
        public RelayCommand EditLineRateEntryCommand { get; private set; }
        public RelayCommand DeleteLineRateEntryCommand { get; private set; }
        public RelayCommand TestDeleteCommand { get; private set; }
        public MainViewModel()
        {
            SelectProfileCommand = new RelayCommand(OpenSelectProfileModal);
            AddProfileCommand = new RelayCommand(OpenAddProfileModal);
            EditProfileCommand = new RelayCommand(OpenEditProfileModal, CanUpdateProfile);
            DeleteProfileCommand = new RelayCommand(ConfirmDeleteProfile, CanUpdateProfile);
            ManageLineRatesCommand = new RelayCommand(OpenManageLineRatesModal, CanManageLineRates);
            AddLineRateEntryCommand = new RelayCommand(OpenAddLineEntryModal, CanAddLineRateEntry);
            EditLineRateEntryCommand = new RelayCommand(OpenEditLineRateEntryModal, CanUpdateLineRateEntry);
            DeleteLineRateEntryCommand = new RelayCommand(ConfirmDeleteLineRateEntry, CanUpdateLineRateEntry);

            editLineRateEntryClosedHandler = new EventHandler(OnEditLineRateEntryClosed);
        }

        private async void OpenManageLineRatesModal()
        {
            manageLineRatesView = new ManageLineRatesView();
            // don't believe an event handler is needed, as this modal doesn't
            // produce any onscreen changes
            //manageLineRatesView.Closed += new EventHandler(OnManageLineRatesClosed);
            manageLineRatesView.ShowDialog();

            await LoadCurrentProfileAsync();

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

        private bool CanAddLineRateEntry()
        {
            // TODO: Add condition to check if datepicker has a valid date selected
            return Profile is ProfileEditModel && (LineRates?.Any() == true);
        }

        private bool CanUpdateProfile()
        {
            return Profile is ProfileEditModel;
        }

        private bool CanManageLineRates()
        {
            return Profile is ProfileEditModel;
        }

        private async void ConfirmDeleteLineRateEntry()
        {
            var response = MessageBox.Show(
                "Delete this record?  This action cannot be undone.",
                "Delete",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (response == MessageBoxResult.OK)
            {
                await App.LineRateEntryDataService.DeleteAsync(SelectedLineRateEntry.LineRateEntryId);
                await LoadLineRateEntriesAsync();
            }
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

        public async Task LoadCurrentProfileAsync()
        {
            ProfileId = Properties.UserSettings.Default.CurrentProfileId;
            Profile = await App.ProfileDataService.GetModelAsync(ProfileId);
            LineRates = await App.LineRateDataService.GetForProfileAsync(ProfileId);

            if (Profile is ProfileEditModel)
            {
                Application.Current.MainWindow.Title = $"TranscripTrack - {Profile.Name} ({Profile.Client})";
            }
            else
            {
                Application.Current.MainWindow.Title = "TranscripTrack";
            }

            await LoadLineRateEntriesAsync();
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            LineEntryDate = DateTime.Today;

            await LoadCurrentProfileAsync();
        }

        public async void OnEntryDateChanged(object sender, SelectionChangedEventArgs e)
        {
            await LoadLineRateEntriesAsync();
        }

        private async Task LoadLineRateEntriesAsync()
        {
            LineRateEntries = await App.LineRateEntryDataService.GetForProfileAndDateAsync(LineEntryDate, ProfileId);
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
            editLineRateEntryView = new EditLineRateEntryView(SelectedLineRateEntry.EnteredDate, SelectedLineRateEntry.LineRateEntryId);
            editLineRateEntryView.Closed += editLineRateEntryClosedHandler;
            editLineRateEntryView.ShowDialog();

            GC.Collect();
        }

        private bool CanUpdateLineRateEntry()
        {
            return SelectedLineRateEntry is LineRateEntryTableModel;
        }
    }
}

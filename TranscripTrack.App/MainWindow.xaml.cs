using System.Windows;
using TranscripTrack.Logic;
using TranscripTrack.App.ViewModels;
using TranscripTrack.App.Views;
using System;
using System.Threading.Tasks;

namespace TranscripTrack.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EntryViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            DataContext = viewModel = new EntryViewModel();

            Loaded += MainWindow_LoadedAsync;
            profileSelectButton.Click += SelectButton_Click;
            addProfileButton.Click += AddProfileButton_Click;
        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
        {
            new EditProfileView().Show();
        }

        private async void MainWindow_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await LoadProfileAsync();
        }

        private async Task LoadProfileAsync()
        {
            var currentProfileId = Properties.UserSettings.Default.CurrentProfileId;
            if (!await DataService.ProfileExistsAsync(currentProfileId))
            {
                SelectProfile();
            }
            else
            {
                viewModel.ProfileId = currentProfileId;
            }
        }

        private void SelectProfile()
        {
            var selectProfileView = new SelectProfileView();
            selectProfileView.CloseEvent += new EventHandler(ProfileUpdateEvent);
            selectProfileView.ShowDialog();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectProfile();
        }

        private void ProfileUpdateEvent(object sender, EventArgs e)
        {
            viewModel.ProfileId = Properties.UserSettings.Default.CurrentProfileId;
        }
    }
}

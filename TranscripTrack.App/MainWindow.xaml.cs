using System.Windows;
using TranscripTrack.Logic;
using TranscripTrack.App.ViewModels;
using TranscripTrack.App.Views;

namespace TranscripTrack.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new EntryViewModel();

            Loaded += MainWindow_Loaded;
            profileSelectButton.Click += SelectButton_Click;
            addProfileButton.Click += AddProfileButton_Click;
        }

        private void AddProfileButton_Click(object sender, RoutedEventArgs e)
        {
            new EditProfileView().Show();
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var currentProfileId = Properties.UserSettings.Default.CurrentProfileId;
            if (!await DataService.ProfileExistsAsync(currentProfileId))
            {
                (new SelectProfileView()).Show();
            }
            else
            {
                var viewModel = DataContext as EntryViewModel;
                viewModel.ProfileId = currentProfileId;
                await viewModel.InitializeAsync();
            }
        }
            
        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            new SelectProfileView().Show();
        }
    }
}

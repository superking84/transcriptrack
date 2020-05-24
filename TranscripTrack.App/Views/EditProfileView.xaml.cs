using System.Windows;
using TranscripTrack.UI.ViewModels;

namespace TranscripTrack.UI.Views
{
    /// <summary>
    /// Interaction logic for EditProfileView.xaml
    /// </summary>
    public partial class EditProfileView : Window
    {
        public EditProfileView()
        {
            InitializeComponent();

            DataContext = new EditProfileViewModel(true);

            Loaded += Window_Loaded;
            addButton.Click += AddButton_Click;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await (DataContext as EditProfileViewModel).InitializeAsync();
        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            await (DataContext as EditProfileViewModel).AddProfileAsync();
        }
    }
}

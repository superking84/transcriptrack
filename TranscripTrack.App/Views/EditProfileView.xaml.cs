using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for EditProfileView.xaml
    /// </summary>
    public partial class EditProfileView : Window
    {
        public EditProfileView(int? profileId)
        {
            DataContext = new EditProfileViewModel(profileId);

            InitializeComponent();
        }
    }
}

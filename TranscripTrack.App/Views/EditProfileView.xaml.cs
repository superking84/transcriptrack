using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for EditProfileView.xaml
    /// </summary>
    public partial class EditProfileView : Window
    {
        public EditProfileView(bool isAdd)
        {
            DataContext = new EditProfileViewModel(isAdd);

            InitializeComponent();
        }
    }
}

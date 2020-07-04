using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for SelectProfileView.xaml
    /// </summary>
    public partial class SelectProfileView : Window
    {
        public SelectProfileView()
        {
            Owner = App.Current.MainWindow;
            DataContext = new SelectProfileViewModel();

            InitializeComponent();
        }
    }
}

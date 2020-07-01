using System.Windows.Controls;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            DataContext = new MainViewModel();

            InitializeComponent();
        }
    }
}

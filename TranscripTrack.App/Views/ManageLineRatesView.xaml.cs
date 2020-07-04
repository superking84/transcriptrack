using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for ManageLineRatesView.xaml
    /// </summary>
    public partial class ManageLineRatesView : Window
    {
        public ManageLineRatesView()
        {
            Owner = App.Current.MainWindow;
            DataContext = new ManageLineRatesViewModel();

            InitializeComponent();
        }
    }
}

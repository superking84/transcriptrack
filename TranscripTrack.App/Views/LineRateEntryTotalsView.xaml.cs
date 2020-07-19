using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for LineRateEntryTotalsView.xaml
    /// </summary>
    public partial class LineRateEntryTotalsView : Window
    {
        public LineRateEntryTotalsView(int month, int year)
        {
            Owner = App.Current.MainWindow;
            DataContext = new LineRateEntryTotalsViewModel(month, year);

            InitializeComponent();
        }
    }
}

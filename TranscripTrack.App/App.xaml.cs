using System.Windows;
using System.Windows.Controls;
using TranscripTrack.Data;
using TranscripTrack.Logic;

namespace TranscripTrack.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static TrackerDbContext DbContext { get; private set; }
        public static CurrencyDataService CurrencyDataService { get; private set; }
        public static LineRateDataService LineRateDataService { get; private set; }
        public static LineRateEntryDataService LineRateEntryDataService { get; private set; }
        public static ProfileDataService ProfileDataService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DbContext = new TrackerDbContext();

            CurrencyDataService = new CurrencyDataService(DbContext);
            LineRateDataService = new LineRateDataService(DbContext);
            LineRateEntryDataService = new LineRateEntryDataService(DbContext);
            ProfileDataService = new ProfileDataService(DbContext);

            // prevent tooltips from closing on their own
            // https://stackoverflow.com/questions/896574/forcing-a-wpf-tooltip-to-stay-on-the-screen
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), 
                new FrameworkPropertyMetadata(int.MaxValue)
            );
        }
    }
}

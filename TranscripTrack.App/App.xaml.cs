using System.Windows;
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
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
        public TrackerDbContext DbContext { get; private set; }
        public ProfileDataService ProfileDataService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DbContext = new TrackerDbContext();
            ProfileDataService = new ProfileDataService(DbContext);
        }
    }
}

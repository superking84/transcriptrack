using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.App.ViewModels;
using System.Configuration;
using System.ComponentModel;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for SelectProfileView.xaml
    /// </summary>
    public partial class SelectProfileView : Window
    {
        public event EventHandler CloseEvent;
        protected void OnCloseEvent()
        {
            CloseEvent?.Invoke(this, EventArgs.Empty);
        }

        public SelectProfileView()
        {
            InitializeComponent();

            DataContext = new SelectProfileViewModel();

            Loaded += SelectProfileView_LoadedAsync;
            Unloaded += new RoutedEventHandler(SelectProfileView_Unloaded);
            profileSelectTable.MouseDoubleClick += ProfileSelectTable_MouseDoubleClick;
        }

        private void SelectProfileView_Unloaded(object sender, RoutedEventArgs e)
        {
            OnCloseEvent();
        }

        private void ProfileSelectTable_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (profileSelectTable.SelectedItem is ProfileSelectTableModel selectedProfile)
            {
                Properties.UserSettings.Default.CurrentProfileId = selectedProfile.ProfileId;
                Properties.UserSettings.Default.Save();

                this.Close();
            }
        }

        private async void SelectProfileView_LoadedAsync(object sender, RoutedEventArgs e)
        {
            await (DataContext as SelectProfileViewModel).InitializeAsync();
        }

    }
}

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
        private SelectProfileViewModel viewModel;

        public SelectProfileView()
        {
            DataContext = viewModel = new SelectProfileViewModel();

            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            viewModel.InitializeCommand.Execute(null);
        }
    }
}

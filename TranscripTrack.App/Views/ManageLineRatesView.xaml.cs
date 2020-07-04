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
            InitializeComponent();
        }
    }
}

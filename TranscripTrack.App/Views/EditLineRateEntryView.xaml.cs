using System;
using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App.Views
{
    /// <summary>
    /// Interaction logic for EditLineRateEntryView.xaml
    /// </summary>
    public partial class EditLineRateEntryView : Window
    {
        public EditLineRateEntryView(DateTime selectedDate, int? lineRateEntryId)
        {
            DataContext = new EditLineRateEntryViewModel(selectedDate, lineRateEntryId);

            InitializeComponent();
        }
    }
}

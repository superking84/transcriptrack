using System;
using System.Collections.Generic;
using System.Text;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class LineRateEntryTotalsViewModel : BaseViewModel
    {
        private int month;
        private int year;

        private LineRateEntryTotalModel grandTotals;
        public LineRateEntryTotalModel GrandTotals {
            get => grandTotals;
            set {
                grandTotals = value;
                OnPropertyChanged(nameof(GrandTotals));
            }
        }

        private List<LineRateEntryTotalModel> lineRateTotals;
        public List<LineRateEntryTotalModel> LineRateTotals { 
            get => lineRateTotals;
            set {
                lineRateTotals = value;
                OnPropertyChanged(nameof(LineRateTotals));
            }
        }

        public LineRateEntryTotalsViewModel(int month, int year)
        {
            this.month = month;
            this.year = year;

            Title = $"Totals for {Constants.Months[month - 1]} {year}";
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            GrandTotals = await App.LineRateEntryDataService.GetGrandTotalForMonthAsync(month, year, Properties.UserSettings.Default.CurrentProfileId);
            LineRateTotals = await App.LineRateEntryDataService.GetTotalsByLineRateForMonthAsync(month, year, Properties.UserSettings.Default.CurrentProfileId);
        }
    }
}

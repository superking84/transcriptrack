using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class ManageLineRatesViewModel : BaseViewModel
    {
        public RelayCommand<Window> SaveCommand { get; private set; }

        private LineRateEditModel selectedLineRate;
        public LineRateEditModel SelectedLineRate {
            get => selectedLineRate;
            set {
                selectedLineRate = value;
                OnPropertyChanged("SelectedLineRate");
            }
        }

        public string InstructionMessage { get; set; }
        public string DescriptionTooltipTitle { get; set; }
        public string DescriptionTooltipMessage { get; set; }

        public string CPLTooltipTitle { get; set; }
        public string CPLTooltipMessage { get; set; }

        public ManageLineRatesViewModel()
        {
            Title = "Manage Line Rates";

            InstructionMessage = "* Enter one or more pay rates above, then press Save to save your changes.";

            DescriptionTooltipTitle = "Description";
            DescriptionTooltipMessage = "A description of the type of lines being done (regular typing, QA, VR, etc.)";

            CPLTooltipTitle = "Pay Rate (CPL)";
            CPLTooltipMessage = "Pay rate in cents per line.";
            SaveCommand = new RelayCommand<Window>(SaveChangesAsync);
                
            LineRates = new List<LineRateEditModel>();
        }

        private async void SaveChangesAsync(Window window)
        {
            // Do not try to save any new blank entries
            var lineRatesToSave = LineRates
                .Where(lr => !string.IsNullOrWhiteSpace(lr.RateText))
                .ToList();
            await App.LineRateDataService.SaveChangesAsync(lineRatesToSave, Properties.UserSettings.Default.CurrentProfileId);

            window?.Close();
        }

        private List<LineRateEditModel> lineRates;
        public List<LineRateEditModel> LineRates {
            get => lineRates;
            set {
                lineRates = value;
                OnPropertyChanged("LineRates");
            }
        }


        public override async void OnLoaded(object sender, EventArgs e)
        {
            LineRates = await App.LineRateDataService.GetAllForEditAsync(Properties.UserSettings.Default.CurrentProfileId);
        }
    }
}

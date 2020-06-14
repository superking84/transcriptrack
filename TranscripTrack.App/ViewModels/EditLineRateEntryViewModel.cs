using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;
using TranscripTrack.Logic;

namespace TranscripTrack.App.ViewModels
{
    public class EditLineRateEntryViewModel : BaseViewModel
    {
        private readonly bool isAdd;
        private readonly int? lineRateEntryId;
        
        public RelayCommand<Window> SaveCommand { get; private set; }

        private List<LineRate> lineRates;
        public List<LineRate> LineRates {
            get => lineRates;
            set {
                lineRates = value;
                OnPropertyChanged("LineRates");
            }
        }

        private LineRateEntryEditModel model;
        public LineRateEntryEditModel Model {
            get => model;
            set {
                model = value;
                OnPropertyChanged("Model");
            }
        }

        public EditLineRateEntryViewModel(DateTime selectedDate, int? lineRateEntryId)
        {
            isAdd = !lineRateEntryId.HasValue;
            //this.selectedDate = selectedDate;
            this.lineRateEntryId = lineRateEntryId;

            Title = $"{(isAdd ? "Add Lines" : "Edit Lines")} - {selectedDate.ToShortDateString()}";

            Model = new LineRateEntryEditModel { EnteredDate = selectedDate };
            SaveCommand = new RelayCommand<Window>(SaveLineRateEntryAsync, CanSave);
        }

        private async void SaveLineRateEntryAsync(Window window)
        {
            await DataService.SaveLineEntryAsync(Model);

            window?.Close();
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            LineRates = await DataService.GetLineRatesAsync(Properties.UserSettings.Default.CurrentProfileId);
            
            if (!isAdd)
            {
                Model = await DataService.GetLineRateEntryAsync(lineRateEntryId.Value);
            }
        }

        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsInputValid(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool CanSave(Window window)
        {
            return IsInputValid(Model.NumLinesText);
        }

        private bool IsInputValid(string inputText)
        {
            return int.TryParse(inputText, out int _);
        }
    }
}

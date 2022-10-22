using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TranscripTrack.Data;
using TranscripTrack.Data.Models;

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
                OnPropertyChanged(nameof(LineRates));
            }
        }

        private LineRateEntryEditModel model;
        public LineRateEntryEditModel Model {
            get => model;
            set {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        public EditLineRateEntryViewModel(DateTime selectedDate, int? lineRateEntryId)
        {
            isAdd = !lineRateEntryId.HasValue;
            this.lineRateEntryId = lineRateEntryId;

            Title = $"{(isAdd ? "Add" : "Edit")} Lines - {selectedDate.ToShortDateString()}";

            Model = new LineRateEntryEditModel { EnteredDate = selectedDate };
            SaveCommand = new RelayCommand<Window>(SaveLineRateEntryAsync, CanSave);
        }

        private async void SaveLineRateEntryAsync(Window window)
        {
            await App.LineRateEntryDataService.SaveAsync(Model);

            window?.Close();
        }

        public override async void OnLoaded(object sender, EventArgs e)
        {
            LineRates = await App.LineRateDataService.GetForProfileAsync(Properties.UserSettings.Default.CurrentProfileId);

            if (!isAdd)
            {
                Model = await App.LineRateEntryDataService.GetModelAsync(lineRateEntryId.Value);
            }
            else
            {
                // for now, just default to the first option to remove an extra step for the user
                // since it defaults to blank
                Model.LineRateId = LineRates.First().LineRateId;
            }
        }

        public void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsInputValid(e.Text))
            {
                e.Handled = true;
            }
        }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
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

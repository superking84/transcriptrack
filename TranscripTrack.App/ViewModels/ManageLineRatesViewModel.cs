﻿using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Windows;
using TranscripTrack.Data.Models;

namespace TranscripTrack.App.ViewModels
{
    public class ManageLineRatesViewModel : BaseViewModel
    {
        //public RelayCommand AddCommand { get; private set; }
        //public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand<Window> SaveCommand { get; private set; }

        private LineRateEditModel selectedLineRate;
        public LineRateEditModel SelectedLineRate {
            get => selectedLineRate;
            set {
                selectedLineRate = value;
                OnPropertyChanged("SelectedLineRate");
            }
        }

        public ManageLineRatesViewModel()
        {
            Title = "Manage Line Rates";

            //AddCommand = new RelayCommand(AddLineRate);
            //RemoveCommand = new RelayCommand(RemoveLineRate, CanRemove);
            SaveCommand = new RelayCommand<Window>(SaveChangesAsync);

            LineRates = new List<LineRateEditModel>();
        }

        private async void SaveChangesAsync(Window window)
        {
            await App.LineRateDataService.SaveChangesAsync(LineRates, Properties.UserSettings.Default.CurrentProfileId);

            window?.Close();
        }

        //private void RemoveLineRate()
        //{
        //    LineRates.Remove(SelectedLineRate);
        //    SelectedLineRate = null;
        //}

        //private bool CanRemove()
        //{
        //    return SelectedLineRate is LineRateEditModel;
        //}

        //private void AddLineRate()
        //{
        //    LineRates.Add(new LineRateEditModel { ProfileId = Properties.UserSettings.Default.CurrentProfileId });
        //}

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
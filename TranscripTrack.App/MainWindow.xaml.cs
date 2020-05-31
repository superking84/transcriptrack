﻿using System;
using System.Windows;
using TranscripTrack.App.ViewModels;

namespace TranscripTrack.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private EntryViewModel viewModel;

        public MainWindow()
        {
            DataContext = viewModel = new EntryViewModel();
            
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            viewModel.InitializeCommand.Execute(null);
        }
    }
}

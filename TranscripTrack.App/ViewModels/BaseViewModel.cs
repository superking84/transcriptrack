using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TranscripTrack.App.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected string title;
        public string Title {
            get => title;
            set {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

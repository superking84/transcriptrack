using System;
using System.ComponentModel;

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

        public abstract void OnLoaded(object sender, EventArgs e);
    }
}

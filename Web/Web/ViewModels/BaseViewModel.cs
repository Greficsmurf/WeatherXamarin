using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using Web.Models;
using Web.Services;

namespace Web.ViewModels
{

    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<CityResponse> DataStore => DependencyService.Get<IDataStore<CityResponse>>();

        bool isBusy = false;
        bool isLoading = false;
        bool isNewLoading = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        public bool IsNewLoading
        {
            get { return isNewLoading; }
            set { SetProperty(ref isNewLoading, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        public bool IsLoading {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Web.Models;
using Web.Views;
using Web.Services;
using Xamarin.Essentials;
using Web.Interfaces;

namespace Web.ViewModels
{

    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<CityResponse> CityResponses { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command RefreshItemsCommand { get; set; }
        public INotification Notifier => DependencyService.Get<INotification>();

        //public bool isLoading { get; set; }


        public ItemsViewModel()
        {
            Title = "Погода";
            IsLoading = true;
            Items = new ObservableCollection<Item>();
            CityResponses = new ObservableCollection<CityResponse>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            RefreshItemsCommand = new Command(async () => {
                
                if (!await DataStore.initList()) {
                    IsBusy = false;
                } 
            });
            
            MessagingCenter.Subscribe<MockDataStore>(this, "LoadItems", async (obj) => {
                //IsLoading = false;
                await ExecuteLoadItemsCommand();
                IsLoading = false;
            });
            MessagingCenter.Subscribe<NewItemPage>(this, "LoadItems", async (obj) => {
                Console.WriteLine("EXECUTION");
                await ExecuteLoadItemsCommand();
            });

        }
        public async void AddMyCity(String cityName) {

            var response = await DataStore.requestMaker(cityName);
            if (!response.name.Equals("404"))
            {
                    Application.Current.Properties.Add(cityName, cityName);
                    await Application.Current.SavePropertiesAsync();
                    DataStore.AddItem(response);
                    await ExecuteLoadItemsCommand();
                    IsLoading = false;
            }
            else {
                Notifier.ShortAlert("Ваш город не найден");
                IsLoading = false;
            }
        }
        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
            try
            {
                CityResponses.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    CityResponses.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                
            }
        }
    }
}
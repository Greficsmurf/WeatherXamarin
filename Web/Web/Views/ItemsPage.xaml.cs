using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Web.Models;
using Web.Views;
using Web.ViewModels;
using Web.Interfaces;
using Xamarin.Essentials;

namespace Web.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)] 

    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        public INotification Notifier => DependencyService.Get<INotification>();
        public ItemsPage()
        {
            
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
            
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
        

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
        async void AddMyCity_Clicked(object sender, EventArgs e) {

            try
            {
                viewModel.IsLoading = true;
                var request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(10));
                

                //var request = new GeolocationRequest(GeolocationAccuracy.Low, new TimeSpan(10));
                Console.WriteLine("Middle");
                CityByGeo response;
                String cityName;
                var location = await Geolocation.GetLocationAsync(request);
                
                Console.WriteLine("End");

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude.ToString()}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    response = await viewModel.DataStore.requestMakerByCoords(location.Latitude.ToString(), location.Longitude.ToString());
                    cityName = response.results[0].components.city;
                    //var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);
                    //cityName = placemarks.FirstOrDefault().Locality;
                    if (!Application.Current.Properties.ContainsKey(cityName))
                    {
                        viewModel.AddMyCity(cityName);
                    }
                    else {
                        viewModel.IsLoading = false;
                    }
                }
                else {
                    Notifier.ShortAlert("Не удалось определить местоположение, возможно нужно сменить гео-режим");
                    
                    viewModel.IsLoading = false;
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine("Exception == " + ex.ToString());
                Notifier.LongAlert("Включите службу геолокации");

            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            if (viewModel.CityResponses.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
        async void Delete_Clicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "AddItem", Item);
            var button = sender as Button;
            var label = button.Parent.FindByName("CityName") as Label;
            //Console.WriteLine(label.to)
            var toDelete = viewModel.CityResponses.Where(x =>
            {
                return x.name == label.Text;
            }).FirstOrDefault();
          
            Application.Current.Properties.Remove(label.Text);
            await Application.Current.SavePropertiesAsync();
            

            viewModel.DataStore.RemoveItem(toDelete);
            viewModel.LoadItemsCommand.Execute(null);
            Console.WriteLine(label.Text);
            Notifier.ShortAlert("Успешно удалено");
        }
    }
    
}
   

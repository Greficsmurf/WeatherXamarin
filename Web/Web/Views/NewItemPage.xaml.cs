using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;

using Web.Models;
using Web.Services;
using Web.Interfaces;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using System.Runtime.CompilerServices;
using Web.ViewModels;

namespace Web.Views
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]

    public partial class NewItemPage : ContentPage
    {
        public IDataStore<CityResponse> DataStore => DependencyService.Get<IDataStore<CityResponse>>();
        public INotification Notifier => DependencyService.Get<INotification>();
        
        public ObservableCollection<CityResponse> FoundCity { get; set; }
        public Item Item { get; set; }

        public async void SearchSubmit(object sender, EventArgs e)
        {
            SearchBar searchBar = sender as SearchBar;
            viewModel.IsNewLoading = true;
            CityResponse res = await DataStore.requestMaker(searchBar.Text);
            if (!res.name.Equals("404"))
            {
                //Application.Current.Properties.Add(searchBar.Text, searchBar.Text);   
                viewModel.FoundCity.Add(res);
                viewModel.IsNewLoading = false;
            }
            else {
                viewModel.IsNewLoading = false;
                Notifier.ShortAlert("Не найдено");
            }
            
        }
        NewItemViewModel viewModel;
        public NewItemPage()
        {

            InitializeComponent();

            FoundCity = new ObservableCollection<CityResponse>();

            //BindingContext = this;
            BindingContext = viewModel = new NewItemViewModel();
        }

        void Save_Clicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "AddItem", Item);
            var button = sender as Button;
            var label = button.Parent.FindByName("CityName") as Label;
            if (!Application.Current.Properties.ContainsKey(label.Text))
            {
                Application.Current.Properties.Add(label.Text, label.Text);
                Application.Current.SavePropertiesAsync();
                DataStore.AddItem(viewModel.FoundCity.FirstOrDefault(x => x.name.Equals(label.Text)));
                Notifier.ShortAlert("Успешно добавлено");
            }
            
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "LoadItems");
            await Navigation.PopModalAsync();
        }
        



    }
    

}
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using Web.Models;
using Web.Views;
using Web.Services;
using Web.Interfaces;
using System.Linq;

namespace Web.ViewModels
{

    public class NewItemViewModel : BaseViewModel
    {
        //public IDataStore<CityResponse> DataStore => DependencyService.Get<IDataStore<CityResponse>>();
        public INotification Notifier => DependencyService.Get<INotification>();

        public ObservableCollection<CityResponse> FoundCity { get; set; }
        //public bool isLoading { get; set; }
        //public Command SearchSubmitCommand { get; set; }

        public NewItemViewModel()
        {
          
            FoundCity = new ObservableCollection<CityResponse>();
            
        }
        /*public async void SearchSubmit(object sender)
        {
            SearchBar searchBar = sender as SearchBar;

            CityResponse res = await DataStore.requestMaker(searchBar.Text);
            Console.WriteLine(res.name + "HELLO");
            if (!res.name.Equals("404"))
            {
                //Application.Current.Properties.Add(searchBar.Text, searchBar.Text);   
                FoundCity.Add(res);
            }
            else
            {
                Notifier.ShortAlert("Не найдено");
            }


        }*/
        /*void Save_Clicked(object sender, EventArgs e)
        {
            //MessagingCenter.Send(this, "AddItem", Item);
            var button = sender as Button;
            var label = button.Parent.FindByName("CityName") as Label;
            if (!Application.Current.Properties.ContainsKey(label.Text))
            {
                Application.Current.Properties.Add(label.Text, label.Text);
                Application.Current.SavePropertiesAsync();
                DataStore.AddItem(FoundCity.FirstOrDefault(x => x.name.Equals(label.Text)));
                Notifier.ShortAlert("Успешно добавлено");
            }

        }*/
    }
}
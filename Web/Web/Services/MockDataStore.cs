using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Web.Models;
using Xamarin.Forms;

namespace Web.Services
{
    public class MockDataStore : IDataStore<CityResponse>
    {
        readonly String[] cityStack = new String[] {"London", "Ufa", "Moscow", "Berlin"};
        readonly String[] windDirs = new String[] { "Север", "Северо-восток", "Восток", "Юго-восток", "Юг", "Юго-запад", "Запад", "Северо-запад", "Север" };

        private List<CityResponse> cityResponses { get; set; }
        private HttpClient httpClient;
        private HttpRequestMessage reqMessage;
        private bool isLoading = false;
        readonly String API_KEY = "b8dec54a4301f7a6ab7acb3f4ef311f0";
        readonly String API_KEY_COORDS = "54132004b818418bb4b9896627e628c1";
        readonly String RequestByCity = "https://api.openweathermap.org/data/2.5/weather?q={cityname}&appid={appid}&units=metric";
        readonly String RequestByCoords = "https://api.opencagedata.com/geocode/v1/json?key={appid}&q={lat}%2C+{lon}&pretty=1&no_annotations=1";

        public MockDataStore()
        {
            cityResponses = new List<CityResponse>();
            Task.Factory.StartNew(initList);
        }
        public async Task<bool> initList() {
            cityResponses.Clear();
            //Application.Current.Properties.Clear();
            //Application.Current.SavePropertiesAsync();
            if (!isLoading)
            {
                isLoading = true;
                
                foreach (var city in Application.Current.Properties)
                {
                    Console.WriteLine("city.key" + city.Key);

                    CityResponse response = await requestMaker(city.Key);
                    if (!response.name.Equals("404")) cityResponses.Add(response);

                }
                Console.WriteLine("Before loadItems");
                MessagingCenter.Send(this, "LoadItems");
                isLoading = false;

                return true;
            }
            return false;
        }
        public async Task<CityResponse> requestMaker(String cityName) {
            httpClient = new HttpClient();
            reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(RequestByCity.Replace("{cityname}", cityName).Replace("{appid}", API_KEY)),
                Method = HttpMethod.Get
            };
            reqMessage.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(reqMessage);
            if (response.IsSuccessStatusCode) {
                var responseContent = response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<CityResponse>(responseContent.Result);
                Console.WriteLine(responseJson.wind.deg + "CITYANME");
                try
                {
                    responseJson.wind.deg = windDirs[(int)Math.Round(float.Parse(responseJson.wind.deg) / 45)];
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    responseJson.wind.deg = "Нет данных";
                }
                return responseJson;
            }
            return new CityResponse(){ name = "404"};
        }
        public async Task<CityByGeo> requestMakerByCoords(String lat, String lon)
        {
            httpClient = new HttpClient();
            reqMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(RequestByCoords.Replace("{lat}", lat).Replace("{lon}", lon).Replace("{appid}", API_KEY_COORDS)),
                Method = HttpMethod.Get
            };
            reqMessage.Headers.Add("Accept", "application/json");
            HttpResponseMessage response = await httpClient.SendAsync(reqMessage);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<CityByGeo>(responseContent.Result);
                     
                return responseJson;
            }
            return new CityByGeo() {  };
        }


        public void AddItem(CityResponse item)
        {
            cityResponses.Add(item);

        }

        public async Task<bool> UpdateItemAsync(CityResponse item)
        {
            var oldItem = cityResponses.Where((CityResponse arg) => arg.name == item.name).FirstOrDefault();
            cityResponses.Remove(oldItem);
            cityResponses.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string name)
        {
            var oldItem = cityResponses.Where((CityResponse arg) => arg.name == name).FirstOrDefault();
            cityResponses.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public CityResponse GetItem(string name)
        {
            return cityResponses.FirstOrDefault(s => s.name == name);
        }

        public async Task<IEnumerable<CityResponse>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(cityResponses);
        }
        public void RemoveItem(CityResponse item) {
            cityResponses.Remove(item);
        }
    }
}
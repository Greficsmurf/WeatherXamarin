using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public interface IDataStore<T>
    {
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string name);
        T GetItem(string name);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<CityResponse> requestMaker(String cityName);
        Task<bool> initList();
        void RemoveItem(CityResponse item);
        void AddItem(T item);
        Task<CityByGeo> requestMakerByCoords(String lat, String lon);
    }
}

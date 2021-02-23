using OpenWeatherMap.Models;
using OpenWeatherMap.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace OpenWeatherMap
{
    public static class Locations
    {
        public static async Task<List<Country>> GetCountries()
        {
            if (Cache.Countries == null)
            {
                var resourceName = "OpenWeatherMap.Assets.TextResources.country.list.json";
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Cache.Countries = await JsonSerializer.DeserializeAsync<List<Country>>(resource);
                }
            }

            return Cache.Countries;
        }

        public static async Task<List<City>> GetCities(Country country)
        {
            await LoadCityToCache();

            return (from city in Cache.Cities
                    where city.Country == country.Alpha2
                    orderby city.Name
                    select city).ToList();
        }

        public static async Task<City> GetCityById(double id)
        {
            await LoadCityToCache();
            return (from city in Cache.Cities
                    where city.Id == id
                    select city).FirstOrDefault();
        }

        private static async Task LoadCityToCache()
        {
            if (Cache.Cities == null)
            {
                var resourceName = "OpenWeatherMap.Assets.TextResources.city.list.json";
                using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    Cache.Cities = await JsonSerializer.DeserializeAsync<List<City>>(resource);
                }
            }
        }

    }
}

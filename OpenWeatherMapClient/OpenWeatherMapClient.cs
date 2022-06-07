using Newtonsoft.Json;
using OpenWeatherMap.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace OpenWeatherMap
{
    public class OpenWeatherMapClient
    {
        private string key;
        public OpenWeatherMapClient()
        {
            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            key = resource.GetString("OpenWeatherAPIKey");
        }

        public enum Units { Metric, Imperial }
        public async Task<WeatherResponse> GetWeatherAsync(double cityId, Units units = Units.Metric)
        {
            var url = "https://api.openweathermap.org/data/2.5/weather?id=" + cityId + "&appid=" + key + "&units=" + (units == Units.Metric ? "metric" : "imperial");
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherResponse>(json);
            }
        }
    }
}

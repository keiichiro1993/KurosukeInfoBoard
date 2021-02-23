using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenWeatherMap.Models
{
    public class Weather
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("main")]
        public string Main { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }
        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }
        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }
        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }
        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }
        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
        [JsonPropertyName("deg")]
        public int Deg { get; set; }
    }

    public class Clouds
    {
        [JsonPropertyName("all")]
        public int All { get; set; }
    }

    public class Sys
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("country")]
        public string Country { get; set; }
        [JsonPropertyName("sunrise")]
        public int Sunrise { get; set; }
        [JsonPropertyName("sunset")]
        public int Sunset { get; set; }
    }

    public class WeatherResponse
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; }
        [JsonPropertyName("weather")]
        public List<Weather> Weathers { get; set; }
        [JsonPropertyName("base")]
        public string Base { get; set; }
        [JsonPropertyName("main")]
        public Main Main { get; set; }
        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }
        [JsonPropertyName("wind")]
        public Wind Wind { get; set; }
        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; }
        [JsonPropertyName("dt")]
        public int DT { get; set; }
        [JsonPropertyName("sys")]
        public Sys Sys { get; set; }
        [JsonPropertyName("timezone")]
        public int Timezone { get; set; }
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("cod")]
        public int Cod { get; set; }

        public Weather FirstWeather { get { return Weathers.FirstOrDefault(); } }
        public string Icon
        {
            get
            {
                switch (FirstWeather.Icon)
                {
                    case "01d":
                        return "/Assets/Icons/Weather/clear_sky_day.svg";
                    case "01n":
                        return "/Assets/Icons/Weather/clear_sky_night.svg";
                    case "02d":
                        return "/Assets/Icons/Weather/few_clouds_day.svg";
                    case "02n":
                        return "/Assets/Icons/Weather/few_clouds_night.svg";
                    case "03d":
                        return "/Assets/Icons/Weather/scattered_clouds.svg";
                    case "03n":
                        return "/Assets/Icons/Weather/scattered_clouds.svg";
                    case "04d":
                        return "/Assets/Icons/Weather/broken_clouds.svg";
                    case "04n":
                        return "/Assets/Icons/Weather/broken_clouds.svg";
                    case "09d":
                        return "/Assets/Icons/Weather/rain.svg";
                    case "09n":
                        return "/Assets/Icons/Weather/rain.svg";
                    case "10d":
                        return "/Assets/Icons/Weather/rain.svg";
                    case "10n":
                        return "/Assets/Icons/Weather/rain.svg";
                    case "11d":
                        return "/Assets/Icons/Weather/thunderstorm.svg";
                    case "11n":
                        return "/Assets/Icons/Weather/thunderstorm.svg";
                    case "13d":
                        return "/Assets/Icons/Weather/snow.svg";
                    case "13n":
                        return "/Assets/Icons/Weather/snow.svg";
                    case "50d":
                        return "/Assets/Icons/Weather/mist.svg";
                    case "50n":
                        return "/Assets/Icons/Weather/mist.svg";
                    default:
                        return "/Assets/Square150x150Logo.scale-200.png";
                }
            }
        }
    }
}

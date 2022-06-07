using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OpenWeatherMap.Models
{
    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("main")]
        public string Main { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }
        [JsonProperty("feels_like")]
        public double FeelsLike { get; set; }
        [JsonProperty("temp_min")]
        public double TempMin { get; set; }
        [JsonProperty("temp_max")]
        public double TempMax { get; set; }
        [JsonProperty("pressure")]
        public int Pressure { get; set; }
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }
        [JsonProperty("deg")]
        public int Deg { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }
    }

    public class Sys
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("sunrise")]
        public int Sunrise { get; set; }
        [JsonProperty("sunset")]
        public int Sunset { get; set; }
    }

    public class WeatherResponse
    {
        [JsonProperty("coord")]
        public Coord Coord { get; set; }
        [JsonProperty("weather")]
        public List<Weather> Weathers { get; set; }
        [JsonProperty("base")]
        public string Base { get; set; }
        [JsonProperty("main")]
        public Main Main { get; set; }
        [JsonProperty("visibility")]
        public int Visibility { get; set; }
        [JsonProperty("wind")]
        public Wind Wind { get; set; }
        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }
        [JsonProperty("dt")]
        public int DT { get; set; }
        [JsonProperty("sys")]
        public Sys Sys { get; set; }
        [JsonProperty("timezone")]
        public int Timezone { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("cod")]
        public int Cod { get; set; }

        public Weather FirstWeather { get { return Weathers.FirstOrDefault(); } }
        public string Icon
        {
            get
            {
                var path = Application.Current.RequestedTheme == ApplicationTheme.Light ? "/Assets/Icons/Weather_black/" : "/Assets/Icons/Weather_white/";

                switch (FirstWeather.Icon)
                {
                    case "01d":
                        return path + "clear_sky_day.svg";
                    case "01n":
                        return path + "clear_sky_night.svg";
                    case "02d":
                        return path + "few_clouds_day.svg";
                    case "02n":
                        return path + "few_clouds_night.svg";
                    case "03d":
                        return path + "scattered_clouds.svg";
                    case "03n":
                        return path + "scattered_clouds.svg";
                    case "04d":
                        return path + "broken_clouds.svg";
                    case "04n":
                        return path + "broken_clouds.svg";
                    case "09d":
                        return path + "rain.svg";
                    case "09n":
                        return path + "rain.svg";
                    case "10d":
                        return path + "rain.svg";
                    case "10n":
                        return path + "rain.svg";
                    case "11d":
                        return path + "thunderstorm.svg";
                    case "11n":
                        return path + "thunderstorm.svg";
                    case "13d":
                        return path + "snow.svg";
                    case "13n":
                        return path + "snow.svg";
                    case "50d":
                        return path + "mist.svg";
                    case "50n":
                        return path + "mist.svg";
                    default:
                        return "/Assets/Square150x150Logo.scale-200.png";
                }
            }
        }
    }
}

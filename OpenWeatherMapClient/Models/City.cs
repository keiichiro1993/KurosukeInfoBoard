using Newtonsoft.Json;

namespace OpenWeatherMap.Models
{
    public class Coord
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }
        [JsonProperty("lat")]
        public double Latitude { get; set; }
    }

    public class City
    {
        [JsonProperty("id")]
        public double Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("coord")]
        public Coord Coord { get; set; }
    }
}

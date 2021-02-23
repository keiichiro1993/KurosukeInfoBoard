using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenWeatherMap.Models
{
    public class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("alpha-2")]
        public string Alpha2 { get; set; }
        [JsonPropertyName("alpha-3")]
        public string Alpha3 { get; set; }
        [JsonPropertyName("country-code")]
        public string CountryCode { get; set; }
        [JsonPropertyName("iso_3166-2")]
        public string Iso31662 { get; set; }
        [JsonPropertyName("region")]
        public string Region { get; set; }
        [JsonPropertyName("sub-region")]
        public string SubRegion { get; set; }
        [JsonPropertyName("intermediate-region")]
        public string IntermediateRegion { get; set; }
        [JsonPropertyName("region-code")]
        public string RegionCode { get; set; }
        [JsonPropertyName("sub-region-code")]
        public string SubRegionCode { get; set; }
        [JsonPropertyName("intermediate-region-code")]
        public string IntermediateRegionCode { get; set; }
    }
}

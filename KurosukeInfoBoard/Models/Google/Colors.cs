using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Google
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Color
    {
        public string background { get; set; }
        public string foreground { get; set; }
    }

    public class CalendarColors
    {
        [JsonProperty("1")]
        public Color color1 { get; set; }
        [JsonProperty("2")]
        public Color color2 { get; set; }
        [JsonProperty("3")]
        public Color color3 { get; set; }
        [JsonProperty("4")]
        public Color color4 { get; set; }
        [JsonProperty("5")]
        public Color color5 { get; set; }
        [JsonProperty("6")]
        public Color color6 { get; set; }
        [JsonProperty("7")]
        public Color color7 { get; set; }
        [JsonProperty("8")]
        public Color color8 { get; set; }
        [JsonProperty("9")]
        public Color color9 { get; set; }
        [JsonProperty("10")]
        public Color color10 { get; set; }
        [JsonProperty("11")]
        public Color color11 { get; set; }
        [JsonProperty("12")]
        public Color color12 { get; set; }
        [JsonProperty("13")]
        public Color color13 { get; set; }
        [JsonProperty("14")]
        public Color color14 { get; set; }
        [JsonProperty("15")]
        public Color color15 { get; set; }
        [JsonProperty("16")]
        public Color color16 { get; set; }
        [JsonProperty("17")]
        public Color color17 { get; set; }
        [JsonProperty("18")]
        public Color color18 { get; set; }
        [JsonProperty("19")]
        public Color color19 { get; set; }
        [JsonProperty("20")]
        public Color color20 { get; set; }
        [JsonProperty("21")]
        public Color color21 { get; set; }
        [JsonProperty("22")]
        public Color color22 { get; set; }
        [JsonProperty("23")]
        public Color color23 { get; set; }
        [JsonProperty("24")]
        public Color color24 { get; set; }
    }

    public class EventColors
    {
        [JsonProperty("1")]
        public Color color1 { get; set; }
        [JsonProperty("2")]
        public Color color2 { get; set; }
        [JsonProperty("3")]
        public Color color3 { get; set; }
        [JsonProperty("4")]
        public Color color4 { get; set; }
        [JsonProperty("5")]
        public Color color5 { get; set; }
        [JsonProperty("6")]
        public Color color6 { get; set; }
        [JsonProperty("7")]
        public Color color7 { get; set; }
        [JsonProperty("8")]
        public Color color8 { get; set; }
        [JsonProperty("9")]
        public Color color9 { get; set; }
        [JsonProperty("10")]
        public Color color10 { get; set; }
        [JsonProperty("11")]
        public Color color11 { get; set; }
    }

    public class Colors
    {
        public string kind { get; set; }
        public DateTime updated { get; set; }
        public CalendarColors calendar { get; set; }
        public EventColors @event { get; set; }
    }


}

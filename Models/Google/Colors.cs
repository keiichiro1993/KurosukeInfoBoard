using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [JsonPropertyName("1")]
        public Color color1 { get; set; }
        [JsonPropertyName("2")]
        public Color color2 { get; set; }
        [JsonPropertyName("3")]
        public Color color3 { get; set; }
        [JsonPropertyName("4")]
        public Color color4 { get; set; }
        [JsonPropertyName("5")]
        public Color color5 { get; set; }
        [JsonPropertyName("6")]
        public Color color6 { get; set; }
        [JsonPropertyName("7")]
        public Color color7 { get; set; }
        [JsonPropertyName("8")]
        public Color color8 { get; set; }
        [JsonPropertyName("9")]
        public Color color9 { get; set; }
        [JsonPropertyName("10")]
        public Color color10 { get; set; }
        [JsonPropertyName("11")]
        public Color color11 { get; set; }
        [JsonPropertyName("12")]
        public Color color12 { get; set; }
        [JsonPropertyName("13")]
        public Color color13 { get; set; }
        [JsonPropertyName("14")]
        public Color color14 { get; set; }
        [JsonPropertyName("15")]
        public Color color15 { get; set; }
        [JsonPropertyName("16")]
        public Color color16 { get; set; }
        [JsonPropertyName("17")]
        public Color color17 { get; set; }
        [JsonPropertyName("18")]
        public Color color18 { get; set; }
        [JsonPropertyName("19")]
        public Color color19 { get; set; }
        [JsonPropertyName("20")]
        public Color color20 { get; set; }
        [JsonPropertyName("21")]
        public Color color21 { get; set; }
        [JsonPropertyName("22")]
        public Color color22 { get; set; }
        [JsonPropertyName("23")]
        public Color color23 { get; set; }
        [JsonPropertyName("24")]
        public Color color24 { get; set; }
    }

    public class EventColors
    {
        [JsonPropertyName("1")]
        public Color color1 { get; set; }
        [JsonPropertyName("2")]
        public Color color2 { get; set; }
        [JsonPropertyName("3")]
        public Color color3 { get; set; }
        [JsonPropertyName("4")]
        public Color color4 { get; set; }
        [JsonPropertyName("5")]
        public Color color5 { get; set; }
        [JsonPropertyName("6")]
        public Color color6 { get; set; }
        [JsonPropertyName("7")]
        public Color color7 { get; set; }
        [JsonPropertyName("8")]
        public Color color8 { get; set; }
        [JsonPropertyName("9")]
        public Color color9 { get; set; }
        [JsonPropertyName("10")]
        public Color color10 { get; set; }
        [JsonPropertyName("11")]
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

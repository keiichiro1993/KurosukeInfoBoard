using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Microsoft
{
    public class Owner
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class Calendar
    {
        [JsonPropertyName("@odata.id")]
        public string OdataId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string changeKey { get; set; }
        public bool canShare { get; set; }
        public bool canViewPrivateItems { get; set; }
        public string hexColor { get; set; }
        public bool canEdit { get; set; }
        public List<string> allowedOnlineMeetingProviders { get; set; }
        public string defaultOnlineMeetingProvider { get; set; }
        public bool isTallyingResponses { get; set; }
        public bool isRemovable { get; set; }
        public Owner owner { get; set; }
    }

    public class CalendarList
    {
        [JsonPropertyName("@odata.context")]
        public string OdataContext { get; set; }
        public List<Calendar> value { get; set; }
    }
}

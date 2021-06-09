using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class MicrosoftUser : UserBase
    {
        [JsonPropertyName("businessPhones")]
        public List<string> BusinessPhones { get; set; }
        [JsonPropertyName("displayName")]
        public string DisplayName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }
        [JsonPropertyName("givenName")]
        public string GivenName { get; set; }
        [JsonPropertyName("jobTitle")]
        public string JobTitle { get; set; }
        [JsonPropertyName("mail")]
        public string Mail { get; set; }
        [JsonPropertyName("mobilePhone")]
        public string MobilePhone { get; set; }
        [JsonPropertyName("officeLocation")]
        public string OfficeLocation { get; set; }
        [JsonPropertyName("preferredLanguage")]
        public string PreferredLanguage { get; set; }
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [JsonPropertyName("userPrincipalName")]
        public string UserPrincipalName
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
        [JsonPropertyName("id")]
        public string GUID { get; set; }
    }
}

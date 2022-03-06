using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class MicrosoftUser : UserBase
    {
        [JsonProperty("businessPhones")]
        public List<string> BusinessPhones { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }
        [JsonProperty("givenName")]
        public string GivenName { get; set; }
        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty("mail")]
        public string Mail { get; set; }
        [JsonProperty("mobilePhone")]
        public string MobilePhone { get; set; }
        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }
        [JsonProperty("preferredLanguage")]
        public string PreferredLanguage { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName
        {
            get { return base.Id; }
            set { base.Id = value; }
        }
        [JsonProperty("id")]
        public string GUID { get; set; }
    }
}

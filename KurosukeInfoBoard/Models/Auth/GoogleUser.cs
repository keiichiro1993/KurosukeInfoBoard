using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class GoogleUser : UserBase
    {
        [JsonProperty("sub")]
        public string Sub { get; set; }
        [JsonProperty("name")]
        public string GoogleName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }
        [JsonProperty("given_name")]
        public string GivenName { get; set; }
        [JsonProperty("family_name")]
        public string FamilyName { get; set; }
        [JsonProperty("picture")]
        public string GoogleProfilePic { set { base.ProfilePictureUrl = value; } }
        [JsonProperty("locale")]
        public string Locale { get; set; }
        [JsonProperty("email")]
        public string Email { set { base.Id = value; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class GoogleUser : UserBase
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
        [JsonPropertyName("name")]
        public string GoogleName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }
        [JsonPropertyName("picture")]
        public string GoogleProfilePic { set { base.ProfilePictureUrl = value; } }
        [JsonPropertyName("locale")]
        public string Locale { get; set; }
        [JsonPropertyName("email")]
        public string Email { set { base.Id = value; } }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class GoogleToken : TokenBase
    {
        [JsonProperty("access_token")]
        public string GoogleAccessToken { set { base.AccessToken = value; } }
        [JsonProperty("expires_in")]
        public int ExpiresIn { set { base.TokenExpiration = DateTime.Now.AddSeconds(value); } }
        [JsonProperty("refresh_token")]
        public string GoogleRefreshToken { set { base.RefreshToken = value; } }
        [JsonProperty("scope")]
        public string Scope { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}

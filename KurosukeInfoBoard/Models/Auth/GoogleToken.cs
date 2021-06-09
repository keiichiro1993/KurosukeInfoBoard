using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class GoogleToken : TokenBase
    {
        [JsonPropertyName("access_token")]
        public string GoogleAccessToken { set { base.AccessToken = value; } }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { set { base.TokenExpiration = DateTime.Now.AddSeconds(value); } }
        [JsonPropertyName("refresh_token")]
        public string GoogleRefreshToken { set { base.RefreshToken = value; } }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class NatureRemoUser : UserBase
    {
        [JsonPropertyName("nickname")]
        public string Nickname { set { base.UserName = value; } }
        [JsonPropertyName("id")]
        public string RemoId { set { base.Id = value; } }
    }
}

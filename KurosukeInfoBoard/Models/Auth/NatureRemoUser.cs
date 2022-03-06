using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class NatureRemoUser : UserBase
    {
        [JsonProperty("nickname")]
        public string Nickname { set { base.UserName = value; } }
        [JsonProperty("id")]
        public string RemoId { set { base.Id = value; } }
    }
}

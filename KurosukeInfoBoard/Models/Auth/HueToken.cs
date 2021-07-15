using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class HueToken : TokenBase
    {
        public HueToken(string key, string bridgeId)
        {
            AccessToken = key;
            UserType = UserType.Hue;
            Id = bridgeId;
        }

        public HueToken() { }
    }
}

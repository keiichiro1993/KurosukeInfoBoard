using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class TokenBase
    {
        public UserType UserType { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }

        public string Id { get; set; }

        public bool IsTokenExpired()
        {
            if (TokenExpiration == null) return true;
            return !(TokenExpiration - DateTime.Now > new TimeSpan(0, 0, 10));
        }

        public async Task<TokenBase> AcquireNewToken()
        {
            switch (UserType)
            {
                case UserType.Google:
                    var newToken = await GoogleAuthClient.AcquireNewTokenWithRefreshToken(RefreshToken);
                    AccessToken = newToken.AccessToken;
                    RefreshToken = newToken.RefreshToken;
                    TokenExpiration = TokenExpiration;
                    return newToken;
                case UserType.Microsoft:
                    break;
                case UserType.MicrosoftOrg:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}

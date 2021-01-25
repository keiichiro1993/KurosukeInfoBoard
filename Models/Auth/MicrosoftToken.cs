using KurosukeInfoBoard.Utils;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class MicrosoftToken : TokenBase
    {
        public AuthenticationResult AuthResult;
        private MicrosoftAuthClient authClient;

        public MicrosoftToken(IAccount account = null)
        {
            base.UserType = UserType.Microsoft;

            // TODO: リソースファイルにする
            authClient = new MicrosoftAuthClient("bd0dd05a-5e9a-4e72-b7f1-130779a479d3", new string[] { "user.read", "Calendars.Read" }, "msalbd0dd05a-5e9a-4e72-b7f1-130779a479d3://auth", account);
        }

        public async override Task<TokenBase> AcquireNewToken()
        {
            AuthResult = await authClient.GetAccessToken();
            base.AccessToken = AuthResult.AccessToken;
            base.Id = AuthResult.Account.Username; //UPN
            base.TokenExpiration = AuthResult.ExpiresOn.DateTime;

            return this;
        }
    }
}

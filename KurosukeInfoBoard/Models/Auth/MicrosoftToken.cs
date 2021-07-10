using KurosukeInfoBoard.Utils;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace KurosukeInfoBoard.Models.Auth
{
    public class MicrosoftToken : TokenBase
    {
        public AuthenticationResult AuthResult;
        private MicrosoftAuthClient authClient;

        public MicrosoftToken(IAccount account = null)
        {
            base.UserType = UserType.Microsoft;

            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            var clientID = resource.GetString("MicrosoftClientID");
            var redirectUrl = resource.GetString("MicrosoftRedirectUrl");
            authClient = new MicrosoftAuthClient(clientID, new string[] { "user.read", "Calendars.Read" }, redirectUrl, account);
        }

        public async override Task<TokenBase> AcquireNewToken()
        {
            AuthResult = await authClient.GetAccessToken();
            base.AccessToken = AuthResult.AccessToken;
            base.Id = AuthResult.Account.Username; //UPN
            base.TokenExpiration = AuthResult.ExpiresOn.DateTime;

            return this;
        }

        public async Task DeleteCachedAccount()
        {
            await authClient.DeleteCachedAccount();
        }
    }
}

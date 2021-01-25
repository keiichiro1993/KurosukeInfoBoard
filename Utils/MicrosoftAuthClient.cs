using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http;
using System.Net.Http.Headers;

namespace KurosukeInfoBoard.Utils
{
    public class MicrosoftAuthClient
    {
        private IPublicClientApplication msalClient;
        private string[] scopes;
        private IAccount account;

        public MicrosoftAuthClient(string appId, string[] scopes, string redirectUrl, IAccount account)
        {
            this.scopes = scopes;
            this.account = account;

            msalClient = PublicClientApplicationBuilder
                .Create(appId)
                .WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount, true)
                .WithRedirectUri(redirectUrl)
                .WithLogging((level, message, containsPii) =>
                {
                    DebugHelper.WriteDebugLog($"MSAL: {level} {message} ");
                }, Microsoft.Identity.Client.LogLevel.Warning, enablePiiLogging: false, enableDefaultPlatformLogging: true)
                .Build();
        }

        //https://docs.microsoft.com/ja-jp/azure/active-directory/develop/scenario-desktop-acquire-token?tabs=dotnet
        public async Task<AuthenticationResult> GetAccessToken()
        {
            AuthenticationResult result;
            try
            {
                result = await msalClient.AcquireTokenSilent(scopes, account).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                result = await msalClient.AcquireTokenInteractive(scopes).ExecuteAsync();
            }

            account = result.Account;
            return result;
        }

        public async Task<IEnumerable<IAccount>> GetCachedAccounts()
        {
            return await msalClient.GetAccountsAsync().ConfigureAwait(false);
        }
    }
}

using KurosukeInfoBoard.Models.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;

namespace KurosukeInfoBoard.Utils
{
    public class GoogleAuthClient
    {
        //Auth code from https://github.com/googlesamples/oauth-apps-for-windows

        /// <summary>
        /// OAuth 2.0 client configuration.
        /// </summary>
        //TODO: hold this in resw file and exclude from git.
        const string redirectURI = "net.kurosuke-coins.infob:/oauth2redirect";
        const string authorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        const string tokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        const string refreshTokenEndpoint = "https://oauth2.googleapis.com/token";

        public static Uri GenerateAuthURI()
        {
            // Generates state and PKCE values.
            string state = randomDataBase64url(32);
            string code_verifier = randomDataBase64url(32);
            string code_challenge = base64urlencodeNoPadding(sha256(code_verifier));
            const string code_challenge_method = "S256";

            // Stores the state and code_verifier values into local settings.
            // Member variables of this class may not be present when the app is resumed with the
            // authorization response, so LocalSettings can be used to persist any needed values.

            //TODO: deplicate using local settings for saving state. use instance variable instead.
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["google_state"] = state;
            localSettings.Values["google_code_verifier"] = code_verifier;

            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            var clientID = resource.GetString("GoogleClientID");

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile%20email%20https://www.googleapis.com/auth/calendar&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                authorizationEndpoint,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                state,
                code_challenge,
                code_challenge_method);

            // Opens the Authorization URI in the browser.
            //var success = Windows.System.Launcher.LaunchUriAsync(new Uri(authorizationRequest));
            return new Uri(authorizationRequest);
        }

        public static async Task<GoogleUser> GetUserAndTokenFromUri(Uri uri)
        {
            DebugHelper.WriteDebugLog("received URI from authentication. URI:" + uri.ToString());
            string queryString = uri.Query;

            // Parses URI params into a dictionary
            // ref: http://stackoverflow.com/a/11957114/72176
            Dictionary<string, string> queryStringParams =
                    queryString.Substring(1).Split('&')
                         .ToDictionary(c => c.Split('=')[0],
                                       c => Uri.UnescapeDataString(c.Split('=')[1]));

            if (queryStringParams.ContainsKey("error"))
            {
                throw new InvalidOperationException(String.Format("OAuth authorization error: {0}.", queryStringParams["error"]));
            }

            if (!queryStringParams.ContainsKey("code")
                || !queryStringParams.ContainsKey("state"))
            {
                throw new InvalidOperationException("Malformed authorization response. " + queryString);
            }

            // Gets the Authorization code & state
            string code = queryStringParams["code"];
            string incoming_state = queryStringParams["state"];

            // Retrieves the expected 'state' value from local settings (saved when the request was made).
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            string expected_state = (String)localSettings.Values["google_state"];

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization
            if (incoming_state != expected_state)
            {
                throw new InvalidOperationException(String.Format("Received request with invalid state ({0})", incoming_state));
            }

            // Resets expected state value to avoid a replay attack.
            localSettings.Values["google_state"] = null;


            string code_verifier = (String)localSettings.Values["google_code_verifier"];
            var token = await performCodeExchangeAsync(code, code_verifier);
            token.UserType = UserType.Google;

            var googleClient = new GoogleClient(token);
            return await googleClient.GetUserData();
        }

        public static async Task<GoogleToken> AcquireNewTokenWithRefreshToken(string refreshToken)
        {
            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            var clientID = resource.GetString("GoogleClientID");

            string tokenRequestBody = string.Format("grant_type=refresh_token&client_id={0}&refresh_token={1}",
                clientID,
                refreshToken);
            DebugHelper.WriteDebugLog("Token refresh requesy body:" + tokenRequestBody);
            return await postRequest(tokenRequestBody, refreshTokenEndpoint);
        }

        private static async Task<GoogleToken> performCodeExchangeAsync(string code, string code_verifier)
        {
            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            var clientID = resource.GetString("GoogleClientID");

            // Builds the Token request
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&scope=&grant_type=authorization_code",
                code,
                System.Uri.EscapeDataString(redirectURI),
                clientID,
                code_verifier
                );
            DebugHelper.WriteDebugLog("Token acquisition requesy body:" + tokenRequestBody);
            return await postRequest(tokenRequestBody, tokenEndpoint);
        }

        private static async Task<GoogleToken> postRequest(string tokenRequestBody, string url)
        {
            StringContent content = new StringContent(tokenRequestBody, Encoding.UTF8, "application/x-www-form-urlencoded");

            // Performs the authorization code exchange.
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = true;
            using (HttpClient client = new HttpClient(handler))
            {
                DebugHelper.WriteDebugLog("Requesting tokens...");
                HttpResponseMessage response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Failed to acquire token from Google. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                }

                // Sets the Authentication header of our HTTP client using the acquired access token.
                string responseString = await response.Content.ReadAsStringAsync();
                DebugHelper.WriteDebugLog("Token acquisition request finished. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                return JsonSerializer.Deserialize<GoogleToken>(responseString);
            }
        }

        private static string randomDataBase64url(uint length)
        {
            IBuffer buffer = CryptographicBuffer.GenerateRandom(length);
            return base64urlencodeNoPadding(buffer);
        }

        private static string base64urlencodeNoPadding(IBuffer buffer)
        {
            string base64 = CryptographicBuffer.EncodeToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        private static IBuffer sha256(string inputString)
        {
            HashAlgorithmProvider sha = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256);
            IBuffer buff = CryptographicBuffer.ConvertStringToBinary(inputString, BinaryStringEncoding.Utf8);
            return sha.HashData(buff);
        }
    }
}

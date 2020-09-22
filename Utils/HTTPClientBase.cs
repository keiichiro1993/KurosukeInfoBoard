using KurosukeInfoBoard.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class HTTPClientBase
    {
        protected TokenBase token;
        protected async Task<string> GetAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        DebugHelper.WriteDebugLog("Get Async failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                        return null;
                    }
                    throw new InvalidOperationException("HTTP request was not successful. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                }

                DebugHelper.WriteDebugLog("Get Async succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

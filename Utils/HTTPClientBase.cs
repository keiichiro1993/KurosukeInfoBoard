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

        protected async Task PostAsync(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    DebugHelper.WriteDebugLog("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                    throw new InvalidOperationException("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                }

                DebugHelper.WriteDebugLog("Post Async succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
            }
        }
    }
}

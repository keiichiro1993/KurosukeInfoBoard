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
        protected async Task<string> GetAsync(string url, int retry = 1)
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
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if (retry > 0)
                        {
                            await token.AcquireNewToken();
                            return await GetAsync(url, retry - 1);
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("Get Async failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                        }
                    }

                    throw new HttpRequestException("Get Async failed.Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                }

                DebugHelper.WriteDebugLog("Get Async succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                return await response.Content.ReadAsStringAsync();
            }
        }

        protected async Task<string> PostAsync(string url, HttpContent content, int retry = 1)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if (retry > 0)
                        {
                            await token.AcquireNewToken();
                            return await PostAsync(url, content, retry - 1);
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("Post Async failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                        }
                    }
                    DebugHelper.WriteDebugLog("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                    throw new HttpRequestException("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase);
                }

                DebugHelper.WriteDebugLog("Post Async succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ".");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

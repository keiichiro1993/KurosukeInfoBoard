using KurosukeInfoBoard.Models.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class HTTPClientBase
    {
        protected TokenBase token;

        protected async Task<T> GetAsyncWithType<T>(string url)
        {
            var jsonString = await GetAsync(url);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        protected async Task<string> GetAsync(string url, int retry = 1)
        {
            DebugHelper.Debugger.WriteDebugLog("GetAsync called. Request URL=" + url + " Available retry=" + retry + ".");
            var response = await GetHttpResponseAsync(url, retry);
            DebugHelper.Debugger.WriteDebugLog("GetAsync succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". ");
            return await response.Content.ReadAsStringAsync();
        }

        protected async Task<HttpResponseMessage> GetHttpResponseAsync(string url, int retry = 1)
        {
            DebugHelper.Debugger.WriteDebugLog("GetHttpResponseAsync called. Request URL=" + url + " Available retry=" + retry + ".");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        DebugHelper.Debugger.WriteDebugLog("GetHttpResponseAsync failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                        return null;
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        if (retry > 0)
                        {
                            await token.AcquireNewToken();
                            return await GetHttpResponseAsync(url, retry - 1);
                        }
                        else
                        {
                            throw new UnauthorizedAccessException("GetHttpResponseAsync failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                        }
                    }

                    throw new HttpRequestException("GetHttpResponseAsync failed.Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                }

                DebugHelper.Debugger.WriteDebugLog("GetHttpResponseAsync succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                return response;
            }
        }

        protected async Task<string> PostAsync(string url, HttpContent content, int retry = 1)
        {
            DebugHelper.Debugger.WriteDebugLog("PostAsync called. Request URL=" + url + " Available retry=" + retry + ".");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
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
                            throw new UnauthorizedAccessException("PostAsync failed. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                        }
                    }
                    DebugHelper.Debugger.WriteDebugLog("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                    throw new HttpRequestException("HTTP request was not successful. Request URL=" + response.RequestMessage.Method + " " + response.RequestMessage.RequestUri.AbsoluteUri + ". HTTP Status=" + response.StatusCode + ". Reason=" + response.ReasonPhrase + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                }

                DebugHelper.Debugger.WriteDebugLog("PostAsync succeeded. Request URL=" + response.RequestMessage.RequestUri.AbsoluteUri + " HTTP Status=" + response.StatusCode + ". Elapsed time=" + stopwatch.Elapsed.TotalSeconds + "secs.");
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

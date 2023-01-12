using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Utils.DBHelpers;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models.Bridge;
using Windows.Networking.Connectivity;
using DebugHelper;
using Newtonsoft.Json.Linq;

namespace KurosukeInfoBoard.Utils
{
    public class HueAuthClient
    {
        public static async Task<IEnumerable<LocatedBridge>> DiscoverHueBridges()
        {
            IBridgeLocator locator = new HttpBridgeLocator();
            return await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
        }

        public static async Task<HueUser> FindHueBridge(TokenBase token)
        {
            //check if there's cached IP address for the bridge
            var bridgeCacheHelper = new HueBridgeCacheHelper();
            var previousIp = await bridgeCacheHelper.GetHueBridgeCachedIp(token.Id);

            if (!string.IsNullOrEmpty(previousIp))
            {
                try
                {
                    return await getBridgeById(token, previousIp);
                }
                catch (Exception ex)
                {
                    // Ignore and fail back to normal discovery
                    Debugger.WriteErrorLog("Hue Bridge discovery with cached IP address failed.", ex);
                }
            }

            IBridgeLocator locator = new HttpBridgeLocator();
            var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            var bridge = (from item in bridges
                          where item.BridgeId == token.Id.ToLower()
                          select item).FirstOrDefault();

            if (bridge != null)
            {
                var client = new LocalHueClient(bridge.IpAddress);
                client.Initialize(token.AccessToken);

                var bridgeInfo = await client.GetBridgeAsync();
                var bridgeId = bridgeInfo.Config.BridgeId;

                var user = new HueUser(bridgeInfo);
                user.Token = token;

                //save if ip address renewed
                if (string.IsNullOrEmpty(previousIp) || previousIp != user.Bridge.Config.IpAddress)
                {
                    await bridgeCacheHelper.SaveHueBridgeCache(token.Id, user.Bridge.Config.IpAddress);
                }

                return user;
            }
            else
            {
                throw new InvalidOperationException("The Hue bridge with ID " + token.Id + " not found in current network.");
            }
        }

        private static async Task<HueUser> getBridgeById(TokenBase token, string ipAddress)
        {
            var client = new LocalHueClient(ipAddress);
            client.Initialize(token.AccessToken);
            var bridgeInfo = await client.GetBridgeAsync();

            var user = new HueUser(bridgeInfo);
            user.Token = token;
            return user;
        }

        public static async Task<HueUser> RegisterHueBridge()
        {
            var bridges = await DiscoverHueBridges();
            var clients = new List<LocalHueClient>();

            foreach (var bridge in bridges)
            {
                clients.Add(new LocalHueClient(bridge.IpAddress));
            }

            var hostname = (from name in NetworkInformation.GetHostNames()
                            where name.Type == Windows.Networking.HostNameType.DomainName
                            select name).First();

            var startTime = DateTime.Now;
            var timeout = new TimeSpan(0, 1, 0);

            while (DateTime.Now - startTime < timeout)
            {
                await Task.Delay(1500);

                foreach (var client in clients)
                {
                    try
                    {
                        var appKey = await client.RegisterAsync("KurosukeInfoBoard", hostname.DisplayName);
                        client.Initialize(appKey);
                        var bridgeInfo = await client.GetBridgeAsync();
                        var bridgeId = bridgeInfo.Config.BridgeId;

                        var token = new HueToken(appKey, bridgeId);
                        var user = new HueUser(bridgeInfo);
                        user.Token = token;

                        return user;
                    }
                    catch (LinkButtonNotPressedException ex)
                    {
                        DebugHelper.Debugger.WriteDebugLog("Hue Bridge link button not yet pressed. " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.Debugger.WriteErrorLog("Error while Hue Bridge registration. This might be because the list contains invalid bridge. Ignoring...", ex);
                    }
                }
            }

            throw new TimeoutException("Hue bridge discovery timed out. Please make sure you pressed the link button within 1 minite.");
        }
    }
}

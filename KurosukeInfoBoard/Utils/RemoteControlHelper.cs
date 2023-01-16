using DebugHelper;
using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.NatureRemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace KurosukeInfoBoard.Utils
{
    public static class RemoteControlHelper
    {
        public static async Task<List<IDevice>> GetAllDevices()
        {
            var taskList = new List<Task<List<IDevice>>>
            {
                GetRemoDevices(),
                GetHueDevices()
            };

            var devicesList = await Task.WhenAll(taskList);

            var mergedDevices = new List<IDevice>();
            foreach (var devices in devicesList) { mergedDevices.AddRange(devices); }
            return mergedDevices;
        }

        public static async Task<List<IDevice>> GetRemoDevices()
        {
            var accounts = from account in AppGlobalVariables.Users
                           where account.UserType == UserType.NatureRemo
                           select account;
            var devices = new List<IDevice>();
            if (accounts.Any())
            {
                var appliances = new List<IAppliance>();
                foreach (var account in accounts)
                {
                    try
                    {
                        var client = new NatureRemoClient(account.Token);
                        devices.AddRange(await client.GetDevicesAsync());
                        appliances.AddRange(await client.GetAppliancesAsync());
                    }
                    catch (Exception ex)
                    {
                        Debugger.WriteErrorLog("Error occured while retrieving remo info.", ex);
                        await new MessageDialog(ex.Message, "Error occured while retrieving remo info.").ShowAsync();
                    }
                }

                if (devices.Any())
                {
                    foreach (var device in devices)
                    {
                        device.Appliances = (from appliance in appliances
                                             where ((Appliance)appliance).device.id == ((Device)device).id
                                             select appliance).ToList();
                    }
                }

            }
            return devices;
        }

        public static async Task<List<IDevice>> GetHueDevices()
        {
            var accounts = from account in AppGlobalVariables.Users
                           where account.UserType == UserType.Hue
                           select account;
            var hueDevices = new List<IDevice>();
            foreach (var account in accounts)
            {
                try
                {
                    var client = new HueClient((HueUser)account);
                    hueDevices.AddRange(await client.GetHueDevicesAsync());
                }
                catch (Exception ex)
                {
                    Debugger.WriteErrorLog("Error occured while retrieving Hue info.", ex);
                    await new MessageDialog(ex.Message, "Error occured while retrieving Hue info.").ShowAsync();
                }
            }

            return hueDevices;
        }
    }
}

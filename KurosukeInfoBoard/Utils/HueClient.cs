﻿using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class HueClient
    {
        private LocalHueClient client;
        private Bridge bridge;
        private HueUser user;
        public HueClient(HueUser user)
        {
            this.user = user;
            bridge = user.Bridge;
            client = new LocalHueClient(bridge.Config.IpAddress);
            client.Initialize(user.Token.AccessToken);
        }

        public async Task<List<IDevice>> GetHueDevicesAsync()
        {
            var hueDevices = new List<IDevice>();
            var groups = await client.GetGroupsAsync();
            var rooms = (from item in groups
                         where item.Type == Q42.HueApi.Models.Groups.GroupType.Room
                         select item).ToList();

            foreach (var room in rooms)
            {
                var hueDevice = new Models.Hue.Group(room);
                var lights = new List<IAppliance>();
                foreach (var lightId in room.Lights)
                {
                    lights.Add(new Models.Hue.Light(await client.GetLightAsync(lightId), user));
                }

                hueDevice.Appliances = lights;
                hueDevices.Add(hueDevice);
            }

            return hueDevices;
        }

        public async Task SendCommandAsync(Light light)
        {
            var command = new LightCommand();

            command.On = light.State.On;
            command.Brightness = light.State.Brightness;

            await client.SendCommandAsync(command, new List<string>() { light.Id });
        }
    }
}

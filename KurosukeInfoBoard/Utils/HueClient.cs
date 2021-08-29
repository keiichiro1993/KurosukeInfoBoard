using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
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

            var scenes = await client.GetScenesAsync();

            foreach (var room in rooms)
            {
                var hueDevice = new Models.Hue.Group(room);
                var lights = new List<IAppliance>();
                foreach (var lightId in room.Lights)
                {
                    lights.Add(new Models.Hue.Light(await client.GetLightAsync(lightId), user));
                }
                hueDevice.Appliances = lights;

                if (scenes != null)
                {
                    hueDevice.HueScenes.AddRange(from scene in scenes
                                                 where scene.Type != null
                                                    && scene.Type == Q42.HueApi.Models.SceneType.GroupScene
                                                    && scene.Group == room.Id
                                                 select scene);
                }

                hueDevices.Add(hueDevice);
            }

            return hueDevices;
        }

        public async Task<Q42.HueApi.Models.Groups.Group> GetHueGroupByIdAsync(string id)
        {
            return await client.GetGroupAsync(id);
        }

        public async Task<Light> SendCommandAsync(Light light, RGBColor? color = null)
        {
            var command = new LightCommand();


            if (color != null)
            {
                command.SetColor((RGBColor)color);
            }

            command.On = light.State.On;
            command.Brightness = light.State.Brightness;

            await client.SendCommandAsync(command, new List<string>() { light.Id });
            return await client.GetLightAsync(light.Id);
        }

        public async Task SendCommandAsync(Q42.HueApi.Models.Groups.Group group)
        {
            var command = new LightCommand();

            command.On = group.Action.On;
            command.Brightness = group.Action.Brightness;

            await client.SendGroupCommandAsync(command, group.Id);
        }

        public async Task SendCommandAsync(Q42.HueApi.Models.Scene scene)
        {
            await client.RecallSceneAsync(scene.Id, scene.Group);
        }
    }
}

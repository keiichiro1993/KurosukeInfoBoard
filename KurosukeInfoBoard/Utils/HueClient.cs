using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KurosukeInfoBoard.Models.Hue.Extensions;
using Newtonsoft.Json;

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

                    // check if scene matches
                    using (var context = new HueSelectedSceneContext())
                    {
                        await context.Database.EnsureCreatedAsync();
                        HueSelectedSceneEntity exist = GetExistingSceneEntity(room.Id, context);
                        if (exist != null)
                        {
                            var match = true;
                            try
                            {
                                var cachedLights = JsonConvert.DeserializeObject<List<Models.Hue.JsonLight>>(exist.LightStateJson);
                                foreach (var cachedLight in cachedLights)
                                {
                                    var light = (from item in hueDevice.Appliances
                                                 where ((Models.Hue.Light)item).HueLight.Id == cachedLight.Id
                                                 select ((Models.Hue.Light)item).HueLight).FirstOrDefault();
                                    if (light != null)
                                    {
                                        if (!light.State.CheckEquals(cachedLight.State))
                                        {
                                            match = false;
                                            break;
                                        }
                                    }
                                }
                            }
                            catch (Exception) 
                            {
                                match = false;
                            }

                            if (match)
                            {
                                hueDevice.SelectedHueScene = (from item in hueDevice.HueScenes
                                                              where item.Id == exist.SceneId
                                                              select item).FirstOrDefault();
                            }
                            else
                            {
                                context.Remove(exist);
                                await context.SaveChangesAsync();
                            }
                        }
                    }

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

            // Clear current scene
            using (var context = new HueSelectedSceneContext())
            {
                HueSelectedSceneEntity exist = GetExistingSceneEntity(group.Id, context);
                if (exist != null)
                {
                    context.Remove(exist);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SendCommandAsync(Q42.HueApi.Models.Scene scene)
        {
            await client.RecallSceneAsync(scene.Id, scene.Group);

            // get status after update
            await Task.Delay(300);
            var lights = new List<Models.Hue.JsonLight>();
            foreach (var light in scene.Lights)
            {
                var lightItem = await client.GetLightAsync(light);
                lights.Add(new Models.Hue.JsonLight(lightItem));
            }

            var lightStateJson = JsonConvert.SerializeObject(lights);

            // Save current scene
            using (var context = new HueSelectedSceneContext())
            {
                HueSelectedSceneEntity exist = GetExistingSceneEntity(scene.Group, context);
                if (exist != null)
                {
                    exist.SceneId = scene.Id;
                    exist.LightStateJson = lightStateJson;
                    context.Update(exist);
                }
                else
                {
                    var entity = new HueSelectedSceneEntity();
                    entity.HueId = bridge.Config.BridgeId;
                    entity.RoomId = scene.Group;
                    entity.SceneId = scene.Id;
                    entity.LightStateJson = lightStateJson;
                    await context.HueSelectedScenes.AddAsync(entity);
                }

                await context.SaveChangesAsync();
            }
        }

        private HueSelectedSceneEntity GetExistingSceneEntity(string groupId, HueSelectedSceneContext context)
        {
            var query = from item in context.HueSelectedScenes
                        where item.HueId == bridge.Config.BridgeId && item.RoomId == groupId
                        select item;

            if (query.Count() > 1)
            {
                context.RemoveRange(query);
            }

            return query.FirstOrDefault();
        }
    }
}

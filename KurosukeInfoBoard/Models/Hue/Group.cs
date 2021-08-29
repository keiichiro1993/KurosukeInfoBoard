using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Models.Hue
{
    public class Group : ViewModelBase, IDevice
    {
        public Group(Q42.HueApi.Models.Groups.Group group)
        {
            DeviceName = group.Name + " - " + group.Class;
            HueGroup = group;
            HueScenes = new List<Q42.HueApi.Models.Scene>();
        }

        Q42.HueApi.Models.Groups.Group HueGroup { get; set; }

        public List<Q42.HueApi.Models.Scene> HueScenes { get; set; }

        public string DeviceName { get; set; }

        public string RoomTemperature { get; set; } = "";

        public string RoomTemperatureUnit { get; set; } = "";

        public List<IAppliance> Appliances { get; set; }

        public Visibility HeaderControlVisibility { get; } = Visibility.Visible;
        public Visibility HeaderTemperatureVisibility { get; } = Visibility.Collapsed;
        public Visibility HeaderSceneControlVisibility { get { return HueScenes.Count > 0 ? Visibility.Visible : Visibility.Collapsed; } }

        public byte HueBrightness
        {
            get { return HueGroup.Action.Brightness; }
            set
            {
                HueGroup.Action.Brightness = value;
                SendGroupCommand();
            }
        }
        public bool HueIsOn
        {
            get { return HueGroup.Action.On; }
            set
            {
                HueGroup.Action.On = value;
                SendGroupCommand();
            }
        }

        private DateTime lastCommand;
        private async void SendGroupCommand()
        {
            lastCommand = DateTime.Now;
            await Task.Delay(1000);
            if (DateTime.Now - lastCommand >= new TimeSpan(0, 0, 1))
            {
                IsLoading = true;
                var appliance = Appliances.FirstOrDefault() as Light;
                if (appliance != null)
                {
                    try
                    {
                        var client = new HueClient(appliance.HueUser);
                        await client.SendCommandAsync(HueGroup);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.Debugger.WriteErrorLog("Error occurred while sending group command.", ex);
                        await new MessageDialog("Error occurred while sending group command: " + ex.Message).ShowAsync();
                    }
                }
                IsLoading = false;
            }
        }



    }
}

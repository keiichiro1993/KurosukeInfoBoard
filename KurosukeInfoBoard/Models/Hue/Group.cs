using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Utils;
using Q42.HueApi.Models;
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
            HueScenes = new List<Scene>();
        }

        Q42.HueApi.Models.Groups.Group _HueGroup;

        Q42.HueApi.Models.Groups.Group HueGroup
        {
            get { return _HueGroup; }
            set
            {
                _HueGroup = value;
                RaisePropertyChanged("HueIsOn");
                RaisePropertyChanged("HueBrightness");
            }
        }

        public List<Scene> HueScenes { get; set; }

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
                if (HueGroup.Action.Brightness != value)
                {
                    HueGroup.Action.Brightness = value;
                    SelectedHueScene = null;
                    SendGroupCommand();
                    RaisePropertyChanged();
                }
            }
        }

        public bool HueIsOn
        {
            get { return HueGroup.Action.On; }
            set
            {
                if (HueGroup.Action.On != value)
                {
                    HueGroup.Action.On = value;
                    SelectedHueScene = null;
                    SendGroupCommand();
                    RaisePropertyChanged();
                }
            }
        }

        private Scene _SelectedHueScene;
        public Scene SelectedHueScene
        {
            get { return _SelectedHueScene; }
            set
            {
                if (_SelectedHueScene != value)
                {
                    _SelectedHueScene = value;
                    SendSceneCommand();
                    RaisePropertyChanged();
                }
            }
        }

        private DateTime lastCommand;
        private async void SendGroupCommand()
        {
            lastCommand = DateTime.Now;
            await Task.Delay(500);
            if (DateTime.Now - lastCommand >= new TimeSpan(0, 0, 0, 0, 500))
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

        private async void SendSceneCommand()
        {
            if (SelectedHueScene != null)
            {
                IsLoading = true;
                var appliance = Appliances.FirstOrDefault() as Light;
                if (appliance != null)
                {
                    try
                    {
                        var client = new HueClient(appliance.HueUser);
                        await client.SendCommandAsync(SelectedHueScene);
                        HueGroup = await client.GetHueGroupByIdAsync(HueGroup.Id);
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.Debugger.WriteErrorLog("Error occurred while sending scene command.", ex);
                        await new MessageDialog("Error occurred while sending scene command: " + ex.Message).ShowAsync();
                    }
                }
                IsLoading = false;
            }
        }
    }
}

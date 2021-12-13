﻿using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.Hue;
using KurosukeInfoBoard.Utils;
using Q42.HueApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels
{
    /// <summary>
    /// MergedRoom class will combine multiple types of devices to one object to control everything at one UI. only for remo+hue so far
    /// This class must have all the functionality from all the remote control account types.
    /// </summary>
    public class CombinedRoom : ViewModelBase, IDevice
    {

        public CombinedRoom(string deviceName, Models.Hue.Group hueDevice, Models.NatureRemo.Device remoDevice)
        {
            Appliances = new List<IAppliance>();
            HueScenes = new List<Scene>();
            DeviceName = deviceName;
            if (hueDevice != null)
            {
                HueGroup = hueDevice.HueGroup;
                Appliances.AddRange(hueDevice.Appliances);
                HueScenes.AddRange(hueDevice.HueScenes);
                SelectedHueScene = hueDevice.SelectedHueScene;
            }
            if (remoDevice != null)
            {
                RemoDevice = remoDevice;
                Appliances.AddRange(remoDevice.Appliances);
            }
        }

        Models.NatureRemo.Device _RemoDevice;
        Models.NatureRemo.Device RemoDevice
        {
            get { return _RemoDevice; }
            set
            {
                _RemoDevice = value;
                RaisePropertyChanged("RoomTemperature");
            }
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

        public string RoomTemperature { get { return RemoDevice != null ? RemoDevice.newest_events.te.val.ToString() : ""; } }

        public string RoomTemperatureUnit { get { return string.IsNullOrEmpty(RoomTemperature) ? "" : "℃"; } }

        public List<IAppliance> Appliances { get; set; }

        public Visibility HeaderTemperatureVisibility { get { return RemoDevice != null ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility HeaderControlVisibility { get { return HueGroup != null ? Visibility.Visible : Visibility.Collapsed; } }
        public Visibility HeaderSceneControlVisibility { get { return HueScenes.Count > 0 ? Visibility.Visible : Visibility.Collapsed; } }

        public byte HueBrightness
        {
            get
            {
                return HueGroup != null ? HueGroup.Action.Brightness : (byte)0;
            }
            set
            {
                if (HueGroup != null && HueGroup.Action.Brightness != value)
                {
                    HueGroup.Action.Brightness = value;
                    SelectedHueScene = null;
                    SendGroupCommand(500);
                    RaisePropertyChanged();
                }
            }
        }

        public bool HueIsOn
        {
            get { return HueGroup != null ? HueGroup.Action.On : false; }
            set
            {
                if (HueGroup != null && HueGroup.Action.On != value)
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
        private async void SendGroupCommand(int delay = 0)
        {
            lastCommand = DateTime.Now;
            await Task.Delay(delay);
            if (delay == 0 || DateTime.Now - lastCommand >= new TimeSpan(0, 0, 0, 0, delay))
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
                if (appliance != null && SelectedHueScene != null)
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
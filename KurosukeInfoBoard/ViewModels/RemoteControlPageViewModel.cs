using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using DebugHelper;
using KurosukeInfoBoard.Models.Common;
using System.Collections.ObjectModel;
using KurosukeInfoBoard.Models.Auth;
using Microsoft.UI.Xaml.Controls;
using KurosukeInfoBoard.Utils.DBHelpers;

namespace KurosukeInfoBoard.ViewModels
{
    public class RemoteControlPageViewModel : Common.ViewModels.ViewModelBase
    {
        CombinedControlHelper dbHelper = new CombinedControlHelper();

        private ObservableCollection<IDevice> _Devices;
        public ObservableCollection<IDevice> Devices
        {
            get { return _Devices; }
            set
            {
                _Devices = value;
                RaisePropertyChanged();
            }
        }

        private bool isInLoop = false;
        public async void Init()
        {
            await dbHelper.Init();
            await Refresh();
            RefreshLoop();
        }

        private async Task Refresh()
        {
            LoadingMessage = "Acquiring control info...";
            IsLoading = true;

            lastUpdate = DateTime.Now;
            Devices = new ObservableCollection<IDevice>();

            var devices = await RemoteControlHelper.GetAllDevices();
            var combinedControls = dbHelper.GetCombinedControls();

            foreach (var combinedControl in combinedControls)
            {
                var remoDevice = (from device in devices
                                  where typeof(Device) == device.GetType() && ((Device)device).id == combinedControl.RemoID
                                  select device).FirstOrDefault();
                var hueDevice = (from device in devices
                                 where typeof(Models.Hue.Group) == device.GetType() && ((Models.Hue.Group)device).HueGroup.Id == combinedControl.HueID
                                 select device).FirstOrDefault();

                var room = new CombinedRoom(combinedControl.DeviceName, hueDevice as Models.Hue.Group, remoDevice as Device);

                if (remoDevice != null) { devices.Remove(remoDevice); }
                if (hueDevice != null) { devices.Remove(hueDevice); }

                Devices.Add(room);
            }

            if (!SettingsHelper.Settings.ShowCombinedRoomOnly.GetValue<bool>())
            {
                foreach (var device in devices) { Devices.Add(device); }
            }

            IsLoading = false;
        }

        public async void RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using (var RefreshCompletionDeferral = args.GetDeferral())
            {
                await Refresh();
            }
        }

        public void StopAutoRefresh()
        {
            isInLoop = false;
        }

        DateTime lastUpdate = DateTime.Now;
        private async void RefreshLoop()
        {
            if (!isInLoop && SettingsHelper.Settings.AutoRefreshControls.GetValue<bool>())
            {
                isInLoop = true;
                var timeSpan = new TimeSpan(0, SettingsHelper.Settings.AutoRefreshControlsInterval.GetValue<int>(), 0);
                while (isInLoop)
                {
                    await Task.Delay(500);
                    if (DateTime.Now - lastUpdate > timeSpan)
                    {
                        await Refresh();
                    }
                }
            }
        }
    }
}

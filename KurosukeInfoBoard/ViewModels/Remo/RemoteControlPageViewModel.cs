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

namespace KurosukeInfoBoard.ViewModels
{
    public class RemoteControlPageViewModel : Common.ViewModels.ViewModelBase
    {
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

        public async void Init()
        {
            LoadingMessage = "Acquiring control info...";
            IsLoading = true;

            Devices = new ObservableCollection<IDevice>();
            var taskList = new List<Task<List<IDevice>>>();

            var remoAccounts = from account in AppGlobalVariables.Users
                               where account.UserType == Models.Auth.UserType.NatureRemo
                               select account;

            taskList.Add(GetRemoInfo(remoAccounts));

            var hueAccounts = from account in AppGlobalVariables.Users
                              where account.UserType == Models.Auth.UserType.Hue
                              select account;

            taskList.Add(GetHueInfo(hueAccounts));

            var devicesList = await Task.WhenAll(taskList);
            foreach (var devices in devicesList)
            {
                foreach (var device in devices) { Devices.Add(device); }
            }

            IsLoading = false;
        }

        public void RefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            using (var RefreshCompletionDeferral = args.GetDeferral())
            {
                Init();
            }
        }

        private async Task<List<IDevice>> GetRemoInfo(IEnumerable<Models.Auth.UserBase> accounts)
        {
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

        private async Task<List<IDevice>> GetHueInfo(IEnumerable<Models.Auth.UserBase> accounts)
        {
            var hueDevices = new List<IDevice>();
            foreach (var account in accounts)
            {
                var client = new HueClient((HueUser)account);
                hueDevices.AddRange(await client.GetHueDevicesAsync());
            }

            return hueDevices;
        }
    }
}

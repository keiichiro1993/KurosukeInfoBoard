using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace KurosukeInfoBoard.ViewModels
{
    public class RemoteControlPageViewModel : ViewModelBase
    {
        private List<Device> _Devices;
        public List<Device> Devices
        {
            get { return _Devices; }
            set
            {
                _Devices = value;
                RaisePropertyChanged();
            }
        }

        private List<Appliance> _Appliances;
        public List<Appliance> Appliances
        {
            get { return _Appliances; }
            set
            {
                _Appliances = value;
                RaisePropertyChanged();
            }
        }

        public async void Init()
        {
            LoadingMessage = "Acquiring remo info...";
            IsLoading = true;

            var accounts = (from account in AppGlobalVariables.Users
                            where account.UserType == Models.Auth.UserType.NatureRemo
                            select account).ToList();

            if (accounts.Count > 0)
            {
                var devices = new List<Device>();
                var appliances = new List<Appliance>();
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
                        DebugHelper.WriteErrorLog("Error occured while retrieving remo info.", ex);
                        await new MessageDialog(ex.Message, "Error occured while retrieving remo info.").ShowAsync();
                    }
                }

                Appliances = appliances;
                Devices = devices;
            }

            IsLoading = false;
        }
    }
}

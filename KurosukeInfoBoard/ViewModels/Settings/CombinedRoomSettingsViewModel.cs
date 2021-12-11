using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Utils.DBHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class CombinedRoomSettingsViewModel : ViewModelBase
    {
        CombinedControlHelper dbHelper = new CombinedControlHelper();

        public async void Init()
        {
            IsLoading = true;
            LoadingMessage = "Retrieving control info...";
            try
            {
                var remoTask = RemoteControlHelper.GetRemoDevices();
                var hueTask = RemoteControlHelper.GetHueDevices();
                await dbHelper.Init();

                RemoDevices = new ObservableCollection<IDevice>(await remoTask);
                HueDevices = new ObservableCollection<IDevice>(await hueTask);
                CombinedControls = dbHelper.GetCombinedControls();

                // filter out in use devices
                var usedRemoIDs = from control in CombinedControls
                                  where !string.IsNullOrEmpty(control.RemoID)
                                  select control.RemoID;
                var usedRemoDevices = (from device in RemoDevices
                                       where usedRemoIDs.Contains(((Models.NatureRemo.Device)device).id)
                                       select device).ToList();
                foreach (var usedRemoDevice in usedRemoDevices) { RemoDevices.Remove(usedRemoDevice); }

                var usedHueIDs = from control in CombinedControls
                                 where !string.IsNullOrEmpty(control.HueID)
                                 select control.HueID;
                var usedHueDevices = (from device in HueDevices
                                      where usedHueIDs.Contains(((Models.Hue.Group)device).HueGroup.Id)
                                      select device).ToList();
                foreach (var usedHueDevice in usedHueDevices) { HueDevices.Remove(usedHueDevice); }
            }
            catch (Exception ex)
            {
                DebugHelper.Debugger.WriteErrorLog("Error occurred while retrieving remote control info.", ex);
                await new MessageDialog("Error occurred while retrieving remote control info: " + ex.Message).ShowAsync();
            }
            IsLoading = false;
        }

        private ObservableCollection<CombinedControlEntity> _CombinedControls;
        public ObservableCollection<CombinedControlEntity> CombinedControls
        {
            get { return _CombinedControls; }
            set
            {
                _CombinedControls = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IDevice> _RemoDevices;
        public ObservableCollection<IDevice> RemoDevices
        {
            get { return _RemoDevices; }
            set
            {
                _RemoDevices = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<IDevice> _HueDevices;
        public ObservableCollection<IDevice> HueDevices
        {
            get { return _HueDevices; }
            set
            {
                _HueDevices = value;
                RaisePropertyChanged();
            }
        }

        public bool ShowCombinedRoomOnly
        {
            get { return SettingsHelper.Settings.ShowCombinedRoomOnly.GetValue<bool>(); }
            set { SettingsHelper.Settings.ShowCombinedRoomOnly.SetValue(value); }
        }

        public async void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            var contentDialog = new Views.ContentDialogs.AddGroupDialog(RemoDevices, HueDevices, CombinedControls);
            await contentDialog.ShowAsync();
        }

        public async Task RemoveGroupItem(CombinedControlEntity entity)
        {
            try
            {
                CombinedControls.Remove(entity);
                await dbHelper.RemoveCombindControl(entity);
            }
            catch (Exception ex)
            {
                DebugHelper.Debugger.WriteErrorLog("Error occurred while removing combined group.", ex);
                await new MessageDialog("Error occurred while removing combined group: " + ex.Message).ShowAsync();
            }
        }
    }
}

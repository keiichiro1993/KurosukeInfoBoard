using Common.ViewModels;
using DebugHelper;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Utils.DBHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
                CombinedControls = await dbHelper.GetCombinedControls();

                // filter out in use devices
                var usedRemoIDs = from control in CombinedControls
                                  where !string.IsNullOrEmpty(control.RemoId)
                                  select control.RemoId;
                var usedRemoDevices = (from device in RemoDevices
                                       where usedRemoIDs.Contains(((Models.NatureRemo.Device)device).id)
                                       select device).ToList();
                foreach (var usedRemoDevice in usedRemoDevices) { RemoDevices.Remove(usedRemoDevice); }

                var usedHueIDs = from control in CombinedControls
                                 where !string.IsNullOrEmpty(control.HueId)
                                 select control.HueId;
                var usedHueDevices = (from device in HueDevices
                                      where usedHueIDs.Contains(((Models.Hue.Group)device).HueGroup.Id)
                                      select device).ToList();
                foreach (var usedHueDevice in usedHueDevices) { HueDevices.Remove(usedHueDevice); }
            }
            catch (Exception ex)
            {
                await Debugger.ShowErrorDialog("Error occurred while retrieving remote control info.", ex);
            }

            // watch for reorder event
            CombinedControls.CollectionChanged += CombinedControls_CollectionChanged;
            IsLoading = false;
        }

        private CombinedControl previouslyRemovedItem = null;
        private async void CombinedControls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // this is the only way to detect reorder as ListView calls Remove/Add when reordering the item.
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                previouslyRemovedItem = e.OldItems[0] as CombinedControl;
                return;
            }

            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                previouslyRemovedItem = null;
                return;
            }
            else if (previouslyRemovedItem?.Id == (e.NewItems[0] as CombinedControl)?.Id)
            {
                // this should mean reordering
                IsLoading = true;
                try
                {
                    var dbHelper = new CombinedControlHelper();
                    await dbHelper.Init();
                    await dbHelper.UpdateCombinedControlOrder(CombinedControls);
                }
                catch (Exception ex)
                {
                    await Debugger.ShowErrorDialog("Failed to save reordered combined controls.", ex);
                }
                IsLoading = false;
            }
        }

        private ObservableCollection<CombinedControl> _CombinedControls;
        public ObservableCollection<CombinedControl> CombinedControls
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

        public async Task RemoveGroupItem(CombinedControl entity)
        {
            try
            {
                CombinedControls.Remove(entity);
                await dbHelper.RemoveCombindControl(entity);
            }
            catch (Exception ex)
            {
                await Debugger.ShowErrorDialog("Error occurred while removing combined group.", ex);
            }
        }
    }
}

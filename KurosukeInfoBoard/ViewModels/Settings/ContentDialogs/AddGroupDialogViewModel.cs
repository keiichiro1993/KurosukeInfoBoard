using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using KurosukeInfoBoard.Utils.DBHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace KurosukeInfoBoard.ViewModels.Settings.ContentDialogs
{
    public class AddGroupDialogViewModel : ViewModelBase
    {
        public ObservableCollection<IDevice> RemoDevices { get; set; }
        public ObservableCollection<IDevice> HueDevices { get; set; }
        private ObservableCollection<CombinedControl> combinedControls { get; set; }
        public AddGroupDialogViewModel(ObservableCollection<IDevice> remoDevices, ObservableCollection<IDevice> hueDevices, ObservableCollection<CombinedControl> combinedControls)
        {
            RemoDevices = remoDevices;
            HueDevices = hueDevices;
            this.combinedControls = combinedControls;
        }

        private string _DeviceName;
        public string DeviceName
        {
            get { return _DeviceName; }
            set
            {
                if (value != _DeviceName)
                {
                    _DeviceName = value;
                    RaisePropertyChanged();
                    validateParams();
                }
            }
        }

        private bool _IsPrimaryButtonEnabled = false;
        public bool IsPrimaryButtonEnabled
        {
            get { return _IsPrimaryButtonEnabled; }
            set
            {
                _IsPrimaryButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool IsSynchronized { get; set; }

        private IDevice _SelectedRemo;
        public IDevice SelectedRemo
        {
            get { return _SelectedRemo; }
            set
            {
                _SelectedRemo = value;
                updateDeviceName(value);
                validateParams();
            }
        }

        private IDevice _SelectedHue;
        public IDevice SelectedHue
        {
            get { return _SelectedHue; }
            set
            {
                _SelectedHue = value;
                updateDeviceName(value);
                validateParams();
            }
        }

        private void updateDeviceName(IDevice selectedItem)
        {
            if (string.IsNullOrEmpty(DeviceName))
            {
                DeviceName = selectedItem.DeviceName;
            }
        }

        private void validateParams()
        {
            IsPrimaryButtonEnabled = !string.IsNullOrEmpty(DeviceName) && (SelectedHue != null || SelectedRemo != null);
        }

        public async Task CreateGroup()
        {
            var newGroup = new CombinedControl();
            newGroup.DeviceName = DeviceName;
            if (SelectedHue != null)
            {
                newGroup.HueId = ((Models.Hue.Group)SelectedHue).HueGroup.Id;
                newGroup.HueName = SelectedHue.DeviceName;
            }
            if (SelectedRemo != null)
            {
                newGroup.RemoId = ((Models.NatureRemo.Device)SelectedRemo).id;
                newGroup.RemoName = SelectedRemo.DeviceName;
            }
            newGroup.IsSynchronized = IsSynchronized;

            var dbHelper = new CombinedControlHelper();
            await dbHelper.Init();
            await dbHelper.AddUpdateCombinedControl(newGroup);

            if (SelectedHue != null) { HueDevices.Remove(SelectedHue); }
            if (SelectedRemo != null) { RemoDevices.Remove(SelectedRemo); }

            combinedControls.Add(newGroup);
        }

    }
}

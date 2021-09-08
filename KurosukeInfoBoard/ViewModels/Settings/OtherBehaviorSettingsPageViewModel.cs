using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class OtherBehaviorSettingsPageViewModel : Common.ViewModels.ViewModelBase
    {
        public bool IsEnabled
        {
            get { return SettingsHelper.Settings.AutoRefreshControls.GetValue<bool>(); }
            set
            {
                SettingsHelper.Settings.AutoRefreshControls.SetValue(value);
                if (value && Period <= 0)
                {
                    Period = 5;
                    RaisePropertyChanged("Period");
                }
            }
        }

        public int Period
        {
            get { return SettingsHelper.Settings.AutoRefreshControlsInterval.GetValue<int>(); }
            set
            {
                if (SettingsHelper.Settings.AutoRefreshControlsInterval.GetValue<int>() != value)
                {
                    SettingsHelper.Settings.AutoRefreshControlsInterval.SetValue(value);
                }
            }
        }

    }
}

using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class OtherBehaviorSettingsPageViewModel : Common.ViewModels.ViewModelBase
    {
        public bool IsEnabled
        {
            get { return SettingsHelper.Settings.AutoRefreshControls.GetValue<bool>(); }
            set
            {
                if (value != IsEnabled)
                {
                    SettingsHelper.Settings.AutoRefreshControls.SetValue(value);
                    if (value && Period <= 0)
                    {
                        Period = 5;
                        RaisePropertyChanged("Period");
                    }
                    RaisePropertyChanged();
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

        public bool IsAlwaysFullScreen
        {
            get { return SettingsHelper.Settings.AlwaysFullScreen.GetValue<bool>(); }
            set
            {
                SettingsHelper.Settings.AlwaysFullScreen.SetValue(value);
                var view = ApplicationView.GetForCurrentView();
                if (!view.IsFullScreenMode && value)
                {
                    view.TryEnterFullScreenMode();
                }
                else if (view.IsFullScreenMode && !value)
                {
                    view.ExitFullScreenMode();
                }
            }
        }
    }
}

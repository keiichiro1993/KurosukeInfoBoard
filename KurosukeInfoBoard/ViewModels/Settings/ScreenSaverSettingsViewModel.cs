using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class ScreenSaverSettingsViewModel : Common.ViewModels.ViewModelBase
    {
        public bool IsEnabled
        {
            get { return SettingsHelper.Settings.IsScreenSaverEnabled.GetValue<bool>(); }
            set
            {
                SettingsHelper.Settings.IsScreenSaverEnabled.SetValue(value);
                SwitchTimer();
            }
        }

        public int Period
        {
            get { return SettingsHelper.Settings.ScreenSaverPeriod.GetValue<int>(); }
            set { SettingsHelper.Settings.ScreenSaverPeriod.SetValue(value); }
        }

        private void SwitchTimer()
        {
            if (IsEnabled)
            {
                if (Period == 0) Period = 10;
                RaisePropertyChanged("Period");

                ScreenSaverTimer.StartTimer();
            }
            else
            {
                ScreenSaverTimer.StopTimer();
            }
        }
    }
}

﻿using KurosukeInfoBoard.Utils;
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

        public bool UseAV1Codec
        {
            get { return SettingsHelper.Settings.UseAV1Codec.GetValue<bool>(); }
            set { SettingsHelper.Settings.UseAV1Codec.SetValue(value); }
        }

        public string PlaylistId
        {
            get { return SettingsHelper.Settings.YouTubePlaylistId.GetValue<string>(); }
            set
            {
                SettingsHelper.Settings.YouTubePlaylistId.SetValue(value);
                SwitchTimer();
            }
        }

        public bool IsAudioEnabled
        {
            get { return SettingsHelper.Settings.IsScreenSaverAudioEnabled.GetValue<bool>(); }
            set { SettingsHelper.Settings.IsScreenSaverAudioEnabled.SetValue(value); }
        }

        public bool IsCachingEnabled
        {
            get { return SettingsHelper.Settings.IsScreenSaverCachingEnabled.GetValue<bool>(); }
            set { SettingsHelper.Settings.IsScreenSaverCachingEnabled.SetValue(value); }
        }

        private void SwitchTimer()
        {
            if (IsEnabled)
            {
                if (Period == 0)
                {
                    Period = 10;
                    RaisePropertyChanged("Period");
                }

                if (string.IsNullOrEmpty(PlaylistId))
                {
                    PlaylistId = "PLVlzi39cwm_lfg9XSQ7OiT1Oqn0R-nFfB";
                    RaisePropertyChanged("PlaylistId");
                }

                ScreenSaverTimer.StartTimer();
            }
            else
            {
                ScreenSaverTimer.StopTimer();
            }
        }
    }
}

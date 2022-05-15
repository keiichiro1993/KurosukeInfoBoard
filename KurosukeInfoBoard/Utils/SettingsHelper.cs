using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DebugHelper;
using Newtonsoft.Json;

namespace KurosukeInfoBoard.Utils
{
    public static class SettingsHelper
    {
        #region public methods
        public static T LoadAppSetting<T>(string key, T defaultValue = default(T))
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            try
            {
                return (T)roamingSettings.Values[key];
            }
            catch (NullReferenceException)
            {
                return defaultValue;
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Exception in LoadAppSetting method.", ex);
                return defaultValue;
            }
        }

        public static void SaveAppSetting<T>(string key, T value)
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[key] = value;
            Debugger.WriteDebugLog("Settings changed:::" + key + "=" + value + ".");
        }

        /// <summary>
        /// Create or Load GUID
        /// This might be used for logging/telemetry in the future.
        /// </summary>
        /// <returns>user specific GUID</returns>
        public static string CreateOrLoadGUID()
        {
            var guid = LoadAppSetting<string>("UserGUID");
            if (String.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                SaveAppSetting<string>("UserGUID", guid);
            }
            return guid;
        }

        public static string DumpSettingsToJson()
        {
            ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            var values = roamingSettings.Values;
            return JsonConvert.SerializeObject(values);
        }

        #endregion

        #region App global settings
        /// <summary>
        /// Specify list of all the settings for consistency.
        /// </summary>
        public enum Settings { WetherUnits, CityId, IsScreenSaverEnabled, ScreenSaverPeriod, YouTubePlaylistId, AutoRefreshControls, AutoRefreshControlsInterval, UseAV1Codec, ShowCombinedRoomOnly, AlwaysFullScreen, LastSelectedPage }

        public static T GetValue<T>(this Settings setting)
        {
            return LoadAppSetting<T>(setting.ToString());
        }

        public static void SetValue<T>(this Settings setting, T value)
        {
            SaveAppSetting(setting.ToString(), value);
        }
        #endregion
    }
}

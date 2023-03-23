using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace KurosukeInfoBoard.Views
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class ScreenSaverPage : Page
    {
        public string PlaylistId = SettingsHelper.Settings.YouTubePlaylistId.GetValue<string>();
        public bool UseAV1Codec = SettingsHelper.Settings.UseAV1Codec.GetValue<bool>();
        public bool EnableAudio = SettingsHelper.Settings.IsScreenSaverAudioEnabled.GetValue<bool>();
        public bool EnableCaching = SettingsHelper.Settings.IsScreenSaverCachingEnabled.GetValue<bool>();

        public ScreenSaverPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (!view.IsFullScreenMode)
            {
                view.TryEnterFullScreenMode();
            }
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode && !SettingsHelper.Settings.AlwaysFullScreen.GetValue<bool>())
            {
                view.ExitFullScreenMode();
            }
            App.TryGoBack();
        }
    }
}

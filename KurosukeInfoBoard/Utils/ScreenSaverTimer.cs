using System;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KurosukeInfoBoard.Utils
{
    public static class ScreenSaverTimer
    {
        private static TimeSpan interval = new TimeSpan(0, 0, 5);
        private static ThreadPoolTimer timer;

        public static void StartTimer()
        {
            if (timer != null)
            {
                timer.Cancel();
            }

            timer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                var period = new TimeSpan(0, 0, SettingsHelper.Settings.ScreenSaverPeriod.GetValue<int>());
                if (DateTime.Now - AppGlobalVariables.LastTouchActivity > period)
                {
                    await AppGlobalVariables.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {
                        if (AppGlobalVariables.Frame.CurrentSourcePageType != typeof(Views.ScreenSaverPage))
                        {
                            AppGlobalVariables.Frame.Navigate(typeof(Views.ScreenSaverPage));
                        }
                    });
                }
            }, interval);

        }

        public static void StopTimer()
        {
            if (timer != null)
            {
                timer.Cancel();
            }
        }
    }
}
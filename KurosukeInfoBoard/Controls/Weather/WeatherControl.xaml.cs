using KurosukeInfoBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Weather
{
    public sealed partial class WeatherControl : UserControl
    {
        WeatherControlViewModel viewModel = new WeatherControlViewModel();
        public WeatherControl()
        {
            this.InitializeComponent();
            this.Loaded += WeatherControl_Loaded;
            this.Unloaded += WeatherControl_Unloaded;
        }

        private void WeatherControl_Unloaded(object sender, RoutedEventArgs e)
        {
            viewModel.StopRefreshing();
        }

        private async void WeatherControl_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.Init();
        }
    }
}

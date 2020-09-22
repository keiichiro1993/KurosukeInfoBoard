using KurosukeInfoBoard.Models.NatureRemo;
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

namespace KurosukeInfoBoard.Controls
{
    public sealed partial class NatureRemoDeviceControl : UserControl
    {
        public NatureRemoDeviceControl()
        {
            this.InitializeComponent();
        }

        public Device Device
        {
            get => (Device)GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value);
        }

        public static readonly DependencyProperty DeviceProperty =
          DependencyProperty.Register(nameof(Device), typeof(Device),
            typeof(NatureRemoDeviceControl), new PropertyMetadata(null, new PropertyChangedCallback(OnDeviceChanged)));

        private static void OnDeviceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cc = d as NatureRemoDeviceControl;
            var device = (Device)e.NewValue;
            cc.deviceNameTextBlock.Text = device.name;
            cc.temparatureTextBlock.Text = device.newest_events.te.val.ToString();
        }

        public event RoutedEventHandler Clicked;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}

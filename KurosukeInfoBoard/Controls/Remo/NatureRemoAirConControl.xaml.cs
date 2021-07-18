using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.NatureRemo;
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

namespace KurosukeInfoBoard.Controls.Remo
{
    public sealed partial class NatureRemoAirConControl : UserControl
    {
        public NatureRemoAirConControlViewModel viewModel { get; set; } = new NatureRemoAirConControlViewModel();
        public NatureRemoAirConControl()
        {
            this.InitializeComponent();
        }

        public IAppliance Appliance
        {
            get => (IAppliance)GetValue(ApplianceProperty);
            set => SetValue(ApplianceProperty, value);
        }

        public static readonly DependencyProperty ApplianceProperty =
          DependencyProperty.Register(nameof(Appliance), typeof(IAppliance),
            typeof(NatureRemoAirConControl), new PropertyMetadata(null, new PropertyChangedCallback(OnApplianceChanged)));

        private static void OnApplianceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cc = d as NatureRemoAirConControl;
            var appliance = (IAppliance)e.NewValue;
            if (appliance.ApplianceType == "AC")
            {
                cc.Visibility = Visibility.Visible;
                cc.viewModel.Init((Appliance)appliance);
            }
        }
    }
}

using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls
{
    public sealed partial class NatureRemoApplianceControl : UserControl
    {
        public NatureRemoApplianceControl()
        {
            this.InitializeComponent();
        }

        public Appliance Appliance
        {
            get => (Appliance)GetValue(ApplianceProperty);
            set => SetValue(ApplianceProperty, value);
        }

        public static readonly DependencyProperty ApplianceProperty =
          DependencyProperty.Register(nameof(Appliance), typeof(Appliance),
            typeof(NatureRemoApplianceControl), new PropertyMetadata(null, new PropertyChangedCallback(OnApplianceChanged)));

        private static void OnApplianceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cc = d as NatureRemoApplianceControl;
            var appliance = (Appliance)e.NewValue;
            cc.applianceNameTextBlock.Text = appliance.nickname;
            cc.applianceTypeTextBlock.Text = appliance.type;
        }

        private void Ellipse_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = e.NewSize.Width;
            (sender as Windows.UI.Xaml.Controls.Button).Height = width;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;
            loadingControl.IsLoading = true;
            try
            {
                switch (Appliance.type)
                {
                    case "IR":
                        if (Appliance.signals.Count == 1)
                        {
                            var client = new NatureRemoClient(Appliance.Token);
                            await client.PostSignal(Appliance.signals.First().id);
                        }
                        else
                        {
                            //リモコンポップアップする
                            buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                        }
                        break;
                    case "TV":
                        buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                        break;
                    case "AC":
                        buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                        break;
                }
            }
            catch (Exception ex)
            {
                DebugHelper.WriteErrorLog("Error occured while retrieving remo info.", ex);
                await new MessageDialog(ex.Message, "Error occured while retrieving remo info.").ShowAsync();
            }
            loadingControl.IsLoading = false;
            this.IsEnabled = true;
        }
    }
}

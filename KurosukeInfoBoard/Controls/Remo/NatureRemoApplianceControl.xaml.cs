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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using DebugHelper;
using KurosukeInfoBoard.Models.Common;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Remo
{
    public sealed partial class NatureRemoApplianceControl : UserControl
    {
        public NatureRemoApplianceControl()
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
            typeof(NatureRemoApplianceControl), new PropertyMetadata(null, new PropertyChangedCallback(OnApplianceChanged)));

        private static void OnApplianceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cc = d as NatureRemoApplianceControl;

            var appliance = (IAppliance)e.NewValue;

            cc.applianceNameTextBlock.Text = appliance.ApplianceName;
            cc.applianceTypeTextBlock.Text = appliance.ApplianceType;

            var image = new SvgImageSource();
            image.UriSource = new Uri(appliance.IconImage);
            cc.applianceIconImage.Source = image;
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
                if (Appliance.GetType() == typeof(Appliance))
                {
                    var appliance = (Appliance)Appliance;
                    switch (appliance.type)
                    {
                        case "IR":
                            if (appliance.signals.Count == 1)
                            {
                                var client = new NatureRemoClient(appliance.Token);
                                await client.PostSignal(appliance.signals.First().id);
                            }
                            else
                            {
                                //リモコンポップアップする
                                buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                            }
                            break;
                        default:
                            buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                            break;
                    }
                }
                else if (Appliance.GetType() == typeof(Models.Hue.Light))
                {
                    buttonsFlyout.ShowAt(this, new FlyoutShowOptions { Placement = FlyoutPlacementMode.BottomEdgeAlignedLeft });
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while retrieving remo info.", ex);
                await new MessageDialog(ex.Message, "Error occured while retrieving remo info.").ShowAsync();
            }
            loadingControl.IsLoading = false;
            this.IsEnabled = true;
        }
    }
}

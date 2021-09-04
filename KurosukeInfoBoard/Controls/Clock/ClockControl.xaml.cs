using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Clock
{
    public sealed partial class ClockControl : UserControl
    {
        public ClockControl()
        {
            this.InitializeComponent();
            this.Loaded += ClockControl_Loaded;
            this.Unloaded += ClockControl_Unloaded;
        }


        private bool loaded = true;
        private async void ClockControl_Loaded(object sender, RoutedEventArgs e)
        {
            loaded = true;
            while (loaded)
            {
                DateTextBlock.Text = DateTime.Now.ToString("yyyy/MM/dd ddd");
                TimeTextBlock.Text = DateTime.Now.ToString("HH:mm");
                await Task.Delay(1000);
            }
        }


        private void ClockControl_Unloaded(object sender, RoutedEventArgs e)
        {
            loaded = false;
        }
    }
}

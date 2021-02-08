using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Utils.DBHelpers;
using KurosukeInfoBoard.Utils.UIHelper;
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

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Calendar
{
    public sealed partial class CalendarSelectionItemControl : UserControl
    {
        public CalendarSelectionItemControl()
        {
            this.InitializeComponent();
        }

        public CalendarBase Calendar
        {
            get => (CalendarBase)GetValue(CalendarProperty);
            set => SetValue(CalendarProperty, value);
        }

        public static readonly DependencyProperty CalendarProperty =
          DependencyProperty.Register(nameof(Calendar), typeof(CalendarBase),
            typeof(CalendarSelectionItemControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCalendarChanged)));

        private static void OnCalendarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as CalendarSelectionItemControl;
            control.colorPickerButton.Background = ColorConverter.HexToBrush(control.Calendar.Color);
        }

        private async void CheckBox_StatusChanged(object sender, RoutedEventArgs e)
        {
            var calCache = new CalendarCacheHelper();
            await calCache.Init();

            //反映されないので
            Calendar.IsEnabled = (bool)((CheckBox)sender).IsChecked;

            await calCache.AddUpdateCalendarCache(Calendar);
        }

        private async void ColorPicker_ColorChanged(Microsoft.UI.Xaml.Controls.ColorPicker sender, Microsoft.UI.Xaml.Controls.ColorChangedEventArgs args)
        {
            var newColor = args.NewColor.ToString().ToUpper();
            var oldColor = Calendar.Color.ToUpper();
            if (oldColor.Length < 9) { oldColor = oldColor.Replace("#", "#FF"); }
            //if (args.NewColor.ToString().ToUpper() != Calendar.Color.ToUpper())
            if (newColor != oldColor)
            {
                Calendar.Color = args.NewColor.ToString();
                await UpdateColor();

                colorPickerButton.Background = ColorConverter.HexToBrush(Calendar.Color);
            }
        }

        private async void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            Calendar.OverrideColor = null;
            colorPickerButton.Background = ColorConverter.HexToBrush(Calendar.Color);
            colorPicker.Color = ColorConverter.HexToColor(Calendar.Color);

            await UpdateColor();
        }

        private async System.Threading.Tasks.Task UpdateColor()
        {
            try
            {
                var calCache = new CalendarCacheHelper();
                await calCache.Init();
                await calCache.AddUpdateCalendarCache(Calendar);
            }
            catch (Exception ex)
            {
                DebugHelper.WriteErrorLog("Error occured while saving calendar settings.", ex);
                await new MessageDialog(ex.Message, "Error occured while saving calendar settings.").ShowAsync();
            }
        }
    }
}

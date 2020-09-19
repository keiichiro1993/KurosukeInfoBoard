using KurosukeInfoBoard.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
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
    public sealed partial class CalendarControl : UserControl
    {
        List<CalendarItemControl> itemControls = new List<CalendarItemControl>();
        Symbol TouchWritingIcon = (Symbol)0xED5F;
        public CalendarControl()
        {
            this.InitializeComponent();
            this.Loaded += CalendarControl_Loaded;
        }

        private void CalendarControl_Loaded(object sender, RoutedEventArgs e)
        {
            FindChildren<CalendarItemControl>(itemControls, rootGrid);

            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                InkDrawingAttributes drawingAttributes = calendarCanvas.InkPresenter.CopyDefaultDrawingAttributes();
                drawingAttributes.Color = Windows.UI.Colors.White;
                calendarCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            }
        }

        public CalendarMonth CalendarMonth
        {
            get => (CalendarMonth)GetValue(CalendarDayProperty);
            set => SetValue(CalendarDayProperty, value);
        }

        public static readonly DependencyProperty CalendarDayProperty =
          DependencyProperty.Register(nameof(CalendarMonth), typeof(CalendarMonth),
            typeof(CalendarControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCalendarDayChanged)));

        private static void OnCalendarDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CalendarControl cc = d as CalendarControl;
            var month = (CalendarMonth)e.NewValue;
            for (var i = 0; i < month.CalendarDays.Count; i++)
            {
                cc.itemControls[i].CalendarDay = month.CalendarDays[i];
            }
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }

        private void EnableTouchWritingToggle_Click(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                calendarCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                calendarCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }
        }

        private void DisableCanvasToggle_Click(object sender, RoutedEventArgs e)
        {
            if (disableCanvasToggleButton.IsChecked == true)
            {
                calendarCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                calendarCanvas.Visibility = Visibility.Visible;
            }
        }
    }
}

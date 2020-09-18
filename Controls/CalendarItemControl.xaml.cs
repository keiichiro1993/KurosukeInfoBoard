﻿using KurosukeInfoBoard.Models;
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
    public sealed partial class CalendarItemControl : UserControl
    {
        public CalendarItemControl()
        {
            this.InitializeComponent();
        }

        public CalendarDay CalendarDay
        {
            get => (CalendarDay)GetValue(CalendarDayProperty);
            set => SetValue(CalendarDayProperty, value);
        }

        public static readonly DependencyProperty CalendarDayProperty =
          DependencyProperty.Register(nameof(CalendarDay), typeof(CalendarDay),
            typeof(CalendarItemControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCalendarDayChanged)));

        private static void OnCalendarDayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CalendarItemControl cc = d as CalendarItemControl;
            var day = (CalendarDay)e.NewValue;
            if (day.Date.Year != 1)
            {
                cc.DateTextBlock.Text = day.Date.Day.ToString();
                cc.EventsListView.ItemsSource = day.Events;
            }
        }

        private void rootGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                ((Grid)sender).Height = e.NewSize.Width;
            }
        }
    }
}

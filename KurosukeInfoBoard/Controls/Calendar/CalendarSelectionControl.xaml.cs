using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace KurosukeInfoBoard.Controls.Calendar
{
    public sealed partial class CalendarSelectionControl : UserControl
    {
        public CalendarSelectionControl()
        {
            this.InitializeComponent();
        }

        public ObservableCollection<UserBase> Users
        {
            get
            {
                var items = (ObservableCollection<UserBase>)GetValue(UsersProperty);
                return new ObservableCollection<UserBase>(from item in items
                                                          where item.ErrorDetail == null && (item.UserType == UserType.Google || item.UserType == UserType.Microsoft)
                                                          select item);
            }
            set { SetValue(UsersProperty, value); }
        }

        public static readonly DependencyProperty UsersProperty =
          DependencyProperty.Register(nameof(Users), typeof(ObservableCollection<UserBase>),
            typeof(CalendarSelectionControl), new PropertyMetadata(null, new PropertyChangedCallback(OnUsersChanged)));

        private static void OnUsersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}

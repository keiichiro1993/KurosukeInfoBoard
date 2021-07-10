using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Google;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace KurosukeInfoBoard.Utils
{
    public class AppGlobalVariables
    {
        public static Uri GoogleAuthResultUri;
        public static ObservableCollection<UserBase> Users;
        public static Colors Colors;

        // Screen Saver
        public static DateTime LastTouchActivity = DateTime.Now;
        public static Frame Frame;
        public static CoreDispatcher Dispatcher;
    }
}

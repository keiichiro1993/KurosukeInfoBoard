using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Google;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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
using muxc = Microsoft.UI.Xaml.Controls;
using DebugHelper;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace KurosukeInfoBoard
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void mainNavigation_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                Frame.Navigate(typeof(Views.Settings.SettingsPage));
            }
            else if (args.InvokedItemContainer != null)
            {
                var navItemTag = args.InvokedItemContainer.Tag.ToString();
                switch (navItemTag)
                {
                    case "DashboardPage":
                        contentFrame.Navigate(typeof(DashboardPage), args.RecommendedNavigationTransitionInfo);
                        break;
                    case "RemoteControlPage":
                        contentFrame.Navigate(typeof(RemoteControlPage), args.RecommendedNavigationTransitionInfo);
                        break;
                }
                //NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                var users = await AccountManager.GetAuthorizedUserList();
                AppGlobalVariables.Users = new ObservableCollection<UserBase>(users);
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while getting user info on MainPage.", ex);
                var message = new MessageDialog(ex.Message, "Error occured while getting user info");
                await message.ShowAsync();
                AppGlobalVariables.Users = new ObservableCollection<UserBase>();
            }

            mainNavigation.SelectedItem = mainNavigation.MenuItems[0];
            contentFrame.Navigate(typeof(DashboardPage));
        }
    }
}

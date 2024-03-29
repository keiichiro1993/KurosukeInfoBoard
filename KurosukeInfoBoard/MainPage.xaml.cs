﻿using KurosukeInfoBoard.Models.Auth;
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
using Windows.UI.ViewManagement;
using KurosukeInfoBoard.ViewModels;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace KurosukeInfoBoard
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MainPageViewModel viewModel { get; set; } = new MainPageViewModel();
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

            var view = ApplicationView.GetForCurrentView();
            if (!view.IsFullScreenMode && SettingsHelper.Settings.AlwaysFullScreen.GetValue<bool>())
            {
                view.TryEnterFullScreenMode();
            }

            viewModel.IsLoading = true;
            viewModel.LoadingMessage = "Loading User Information...";

            if (AppGlobalVariables.Users == null)
            {
                try
                {
                    var users = await AccountManager.GetAuthorizedUserList();

                    foreach (var user in users)
                    {
                        if (user.ErrorDetail != null)
                        {
                            await Debugger.ShowErrorDialog($"Failed to initialize {user.UserType} with ID '{user.UserName}'", user.ErrorDetail);
                        }
                    }

                    AppGlobalVariables.Users = new ObservableCollection<UserBase>(users);
                }
                catch (Exception ex)
                {
                    await Debugger.ShowErrorDialog("Error occured while initializing user info.", ex);
                    AppGlobalVariables.Users = new ObservableCollection<UserBase>();
                }
            }

            if (mainNavigation.SelectedItem == null)
            {
                switch (SettingsHelper.Settings.LastSelectedPage.GetValue<string>())
                {
                    case "DashboardPage":
                        mainNavigation.SelectedItem = mainNavigation.MenuItems[0];
                        contentFrame.Navigate(typeof(DashboardPage));
                        break;
                    case "RemoteControlPage":
                        mainNavigation.SelectedItem = mainNavigation.MenuItems[1];
                        contentFrame.Navigate(typeof(RemoteControlPage));
                        break;
                    default:
                        mainNavigation.SelectedItem = mainNavigation.MenuItems[0];
                        contentFrame.Navigate(typeof(DashboardPage));
                        break;
                }
            }
            else
            {
                if (contentFrame.CurrentSourcePageType == typeof(DashboardPage))
                {
                    mainNavigation.SelectedItem = mainNavigation.MenuItems[0];
                }
                else if (contentFrame.CurrentSourcePageType == typeof(RemoteControlPage))
                {
                    mainNavigation.SelectedItem = mainNavigation.MenuItems[1];
                }
            }

            viewModel.IsLoading = false;
        }
    }
}

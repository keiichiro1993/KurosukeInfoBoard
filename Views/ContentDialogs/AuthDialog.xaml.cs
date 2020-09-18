using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace KurosukeInfoBoard.Views.ContentDialogs
{
    public sealed partial class AuthDialog : ContentDialog
    {
        public AuthDialogViewModel viewModel { get; set; } = new AuthDialogViewModel();
        public AuthDialog()
        {
            this.InitializeComponent();
            this.Closing += ContentDialog_Closing;
        }
        private async void Google_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsButtonAvailable = false;
            viewModel.IsLoading = true;
            try
            {
                await registerGoogleToken();
            }
            catch (Exception ex)
            {
                DebugHelper.WriteErrorLog("Error in registering Google token on AuthDialog.", ex);
            }
            this.Hide();
        }

        private static async Task registerGoogleToken()
        {
            await Windows.System.Launcher.LaunchUriAsync(GoogleAuthClient.GenerateAuthURI());
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await Task.Delay(1000);
            await Task.Run(() =>
            {
                while (AppGlobalVariables.GoogleAuthResultUri == null || string.IsNullOrEmpty(AppGlobalVariables.GoogleAuthResultUri.AbsoluteUri))
                {
                    if (stopwatch.Elapsed > new TimeSpan(0, 5, 0))
                    {
                        stopwatch.Stop();
                        return;
                    } // 5 mins for auth timeout.
                }
            });
            var user = await GoogleAuthClient.GetUserAndTokenFromUri(AppGlobalVariables.GoogleAuthResultUri);
            DebugHelper.WriteDebugLog("Token:" + user.Token.AccessToken);
            AccountManager.SaveUserToVault(user);
            AppGlobalVariables.Users.Add(user);

            AppGlobalVariables.GoogleAuthResultUri = null;//reset
        }

        void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (!viewModel.IsLoading)
            {
                args.Cancel = true;
            }
        }
    }
}

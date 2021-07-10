using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.ViewModels.Settings;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace KurosukeInfoBoard.Views.ContentDialogs
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class AuthDialogMain : Page
    {
        public AuthDialogViewModel viewModel { get; set; } = new AuthDialogViewModel();
        private AuthDialog dialogHost;

        public AuthDialogMain()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            dialogHost = e.Parameter as AuthDialog;
            dialogHost.Closing += ContentDialog_Closing;
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            dialogHost.Closing -= ContentDialog_Closing;
        }

        void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (!viewModel.IsLoading)
            {
                //args.Cancel = true;
            }
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
                DebugHelper.Debugger.WriteErrorLog("Error in registering Google token on AuthDialog.", ex);
            }
            dialogHost.Hide();
        }

        private void NatureRemo_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsButtonAvailable = false;
            Frame.Navigate(typeof(AuthDialogRemo), dialogHost, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
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
            AccountManager.SaveUserToVault(user);
            AppGlobalVariables.Users.Add(user);
            DebugHelper.Debugger.WriteDebugLog("Successfully acquired google token.");

            AppGlobalVariables.GoogleAuthResultUri = null;//reset
        }

        private async void Microsoft_Click(object sender, RoutedEventArgs e)
        {
            viewModel.IsButtonAvailable = false;
            viewModel.IsLoading = true;
            try
            {
                var token = new MicrosoftToken();
                await token.AcquireNewToken();
                var msClient = new MicrosoftClient(token);
                AppGlobalVariables.Users.Add(await msClient.GetUserDataAsync());
            }
            catch (Exception ex)
            {
                DebugHelper.Debugger.WriteErrorLog("Error in acquiring MS token on AuthDialog.", ex);
            }
            dialogHost.Hide();
        }
    }
}

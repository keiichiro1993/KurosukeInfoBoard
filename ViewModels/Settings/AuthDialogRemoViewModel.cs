using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Views.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using DebugHelper;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class AuthDialogRemoViewModel : ViewModelBase
    {
        private AuthDialog dialogHost;
        private string _TokenString;
        public string TokenString
        {
            get { return _TokenString; }
            set
            {
                _TokenString = value;
                if (!string.IsNullOrEmpty(_TokenString))
                {
                    IsButtonEnabled = true;
                }
                else
                {
                    IsButtonEnabled = false;
                }
            }
        }

        private bool _IsButtonEnabled = false;
        public bool IsButtonEnabled
        {
            get { return _IsButtonEnabled; }
            set
            {
                _IsButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public void Init(AuthDialog dialogHost)
        {
            this.dialogHost = dialogHost;
        }

        public async void OKButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            LoadingMessage = "Verifying token...";

            var client = new NatureRemoClient(new NatureRemoToken(TokenString));
            var user = await client.GetUserDataAsync();

            AccountManager.SaveUserToVault(user);
            AppGlobalVariables.Users.Add(user);
            Debugger.WriteDebugLog("Successfully verified Remo token.");

            dialogHost.Hide();
        }
    }
}

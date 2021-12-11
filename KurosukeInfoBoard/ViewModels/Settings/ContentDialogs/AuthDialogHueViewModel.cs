using DebugHelper;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Views.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class AuthDialogHueViewModel : Common.ViewModels.ViewModelBase
    {
        private AuthDialog dialogHost;

        public async void Init(AuthDialog dialogHost)
        {
            this.dialogHost = dialogHost;

            IsLoading = true;
            LoadingMessage = "Please press Link button on your Hue Bridge within 1 minute... We will discover it automatically for you.";

            try
            {
                var user = await HueAuthClient.RegisterHueBridge();

                AccountManager.SaveUserToVault(user);
                AppGlobalVariables.Users.Add(user);
                Debugger.WriteDebugLog("Successfully discovered Hue bridge " + user.Bridge.Config.BridgeId + " at " + user.Bridge.Config.IpAddress + ".");

                dialogHost.Hide();
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Failed to add Hue Bridge.", ex);
                var message = new MessageDialog("Failed to discover Hue Bridge. Exception=" + ex.GetType().ToString() + ex.Message);
                await message.ShowAsync();

                dialogHost.Hide();
            }
        }


    }
}

using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Models.Auth
{
    public enum UserType { Google, MicrosoftOrg, Microsoft, NatureRemo }
    public class UserBase
    {
        public string UserName { get; set; }
        public TokenBase Token { get; set; }
        public UserType UserType { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Id { get; set; }

        public static async Task<UserBase> AcquireUserFromToken(TokenBase token)
        {
            switch (token.UserType)
            {
                case UserType.Google:
                    var googleClient = new GoogleClient(token);
                    return await googleClient.GetUserData();
                case UserType.Microsoft:
                    break;
                case UserType.MicrosoftOrg:
                    break;
                case UserType.NatureRemo:
                    var remoClient = new NatureRemoClient(token); 
                    return await remoClient.GetUserDataAsync();
                default:
                    break;
            }
            return null;
        }

        public async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Are you sure to delete account?", "Are you sure?");
            dialog.Commands.Add(new UICommand("Delete"));
            dialog.Commands.Add(new UICommand("Cancel"));
            dialog.DefaultCommandIndex = 0;
            dialog.CancelCommandIndex = 1;

            var result = await dialog.ShowAsync();

            if (result.Label == "Delete")
            {
                AccountManager.DeleteUser(this);
                AppGlobalVariables.Users.Remove(this);
            }
        }
    }
}

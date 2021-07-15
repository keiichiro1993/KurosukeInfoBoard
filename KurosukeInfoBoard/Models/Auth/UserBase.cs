using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using DebugHelper;

namespace KurosukeInfoBoard.Models.Auth
{
    public enum UserType { Google, Microsoft, NatureRemo, Hue }
    public class UserBase
    {
        public string UserName { get; set; }
        public TokenBase Token { get; set; }
        public UserType UserType { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Id { get; set; }
        public List<Common.CalendarBase> Calendars { get; set; } = new List<Common.CalendarBase>();

        public static async Task<UserBase> AcquireUserFromToken(TokenBase token)
        {
            try
            {
                switch (token.UserType)
                {
                    case UserType.Google:
                        var googleClient = new GoogleClient(token);
                        return await googleClient.GetUserDataAsync();
                    case UserType.Microsoft:
                        var msClient = new MicrosoftClient(token);
                        return await msClient.GetUserDataAsync();
                    case UserType.NatureRemo:
                        var remoClient = new NatureRemoClient(token);
                        return await remoClient.GetUserDataAsync();
                    case UserType.Hue:
                        return await HueAuthClient.FindHueBridge(token);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Exception occured on acquiring user info. Type=" + token.UserType.ToString(), ex);
                //var message = new MessageDialog("Type=" + token.UserType.ToString() + " Exception=" + ex.Message, "Exception occured on acquiring user info.");
                //await message.ShowAsync();
                var user = new UserBase();
                user.Token = token;
                user.UserType = token.UserType;
                user.UserName = "Failed to get user info.";
                user.ProfilePictureUrl = "/Assets/Square150x150Logo.scale-200.png";
                user.Id = token.Id;

                return user;
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
                await AccountManager.DeleteUser(this);
                AppGlobalVariables.Users.Remove(this);
            }
        }
    }
}

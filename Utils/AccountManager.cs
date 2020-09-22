using KurosukeInfoBoard.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace KurosukeInfoBoard.Utils
{
    public class AccountManager
    {
        private const string ResourceNamePrefix = "KInfoBoard.";

        public static async Task<List<UserBase>> GetAuthorizedUserList()
        {
            var userList = new List<UserBase>();
            var taskList = new List<Task>();
            foreach (var type in Enum.GetValues(typeof(UserType)).Cast<UserType>())
            {
                IReadOnlyList<PasswordCredential> credentialList = null;
                var vault = new PasswordVault();
                try
                {
                    credentialList = vault.FindAllByResource(ResourceNamePrefix + type.ToString());
                }
                catch (Exception ex)
                {
                    DebugHelper.WriteErrorLog("There's no credential. Ignoring.", ex);
                }

                if (credentialList != null && credentialList.Count() > 0)
                {
                    DebugHelper.WriteDebugLog(credentialList.Count + "users found from credential vault.");
                    foreach (var cred in credentialList)
                    {
                        cred.RetrievePassword();
                        taskList.Add(acquireAndAddUser(userList, type, cred));
                    }
                }
            }
            await Task.WhenAll(taskList.ToArray());
            DebugHelper.WriteDebugLog("Successfully retrieved tokens for " + userList.Count + " user(s).");
            return userList;
        }

        public static void DeleteUser(UserBase user)
        {
            var vault = new PasswordVault();
            var cred = vault.Retrieve(ResourceNamePrefix + user.UserType.ToString(), user.Id);
            vault.Remove(cred);
        }

        private static async Task acquireAndAddUser(List<UserBase> userList, UserType type, Windows.Security.Credentials.PasswordCredential cred)
        {
            var token = new TokenBase();
            token.UserType = type;
            token.Id = cred.UserName;
            if (type == UserType.NatureRemo)
            {
                token.AccessToken = cred.Password;
            }
            else
            {
                token.RefreshToken = cred.Password;
                token = await token.AcquireNewToken();
            }
            userList.Add(await UserBase.AcquireUserFromToken(token));
        }

        public static void SaveUserToVault(UserBase user)
        {
            var resourceName = ResourceNamePrefix + user.UserType.ToString();
            var vault = new PasswordVault();
            if (user.UserType == UserType.NatureRemo)
            {
                vault.Add(new PasswordCredential(resourceName, user.Id, user.Token.AccessToken));
            }
            else
            {
                vault.Add(new PasswordCredential(resourceName, user.Id, user.Token.RefreshToken));
            }
        }
    }
}

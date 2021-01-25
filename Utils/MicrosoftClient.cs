using KurosukeInfoBoard.Models.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class MicrosoftClient : HTTPClientBase
    {
        public MicrosoftClient(TokenBase token)
        {
            this.token = token;
        }

        public async Task<MicrosoftUser> GetUserDataAsync()
        {
            var url = "https://graph.microsoft.com/v1.0/me";
            var user = await GetAsyncWithType<MicrosoftUser>(url);
            user.UserType = UserType.Microsoft;
            user.Token = token;

            try
            {
                var response = await GetHttpResponseAsync("https://graph.microsoft.com/v1.0/me/photo/$value");
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    MemoryStream ms = new MemoryStream();
                    responseStream.CopyTo(ms);
                    byte[] buffer = ms.ToArray();
                    string result = Convert.ToBase64String(buffer);
                    user.ProfilePictureUrl = String.Format("data:image/gif;base64,{0}", result);
                    responseStream.Close();
                }
            }
            catch (Exception)
            {
                user.ProfilePictureUrl = "/Assets/Square150x150Logo.scale-200.png";
            }

            return user;
        }
    }
}

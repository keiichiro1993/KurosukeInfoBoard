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

        public async Task<Models.Microsoft.CalendarList> GetCalendarList()
        {
            var url = "https://graph.microsoft.com/v1.0/me/calendars";
            return await GetAsyncWithType<Models.Microsoft.CalendarList>(url);
        }

        public async Task<Models.Microsoft.EventList> GetEventList(Models.Microsoft.Calendar calendar, DateTime month)
        {
            var start = new DateTime(month.Year, month.Month, 1).ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
            var nextMonth = month.AddMonths(1);
            var end = new DateTime(nextMonth.Year, nextMonth.Month, 1).ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
            var url = "https://graph.microsoft.com/v1.0/me/calendars/" + calendar.id + "/events?$filter=start/dateTime ge \'" + start + "\' and start/dateTime lt \'" + end + "\'";

            return await GetAsyncWithType<Models.Microsoft.EventList>(url);
        }
    }
}

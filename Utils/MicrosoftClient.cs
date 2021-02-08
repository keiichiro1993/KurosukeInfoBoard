using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
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
        private UserBase user;
        public MicrosoftClient(TokenBase token)
        {
            var user = new UserBase();
            user.Token = token;
            user.UserType = token.UserType;
            user.UserName = "Failed to get user info.";
            user.ProfilePictureUrl = "/Assets/Square150x150Logo.scale-200.png";
            user.Id = token.Id;
            this.user = user;
            this.token = user.Token;
        }

        public MicrosoftClient(UserBase user)
        {
            this.user = user;
            this.token = user.Token;
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

            this.user = user;
            return user;
        }

        public async Task<Models.Microsoft.CalendarList> GetCalendarList()
        {
            var url = "https://graph.microsoft.com/v1.0/me/calendars";
            var calendars = await GetAsyncWithType<Models.Microsoft.CalendarList>(url);
            foreach (var calendar in calendars.value)
            {
                calendar.AccountType = UserType.Microsoft.ToString();
                calendar.UserId = user.Id;
            }
            return calendars;
        }

        public async Task<Models.Microsoft.EventList> GetEventList(CalendarBase calendar, DateTime month)
        {
            var start = new DateTime(month.Year, month.Month, 1).ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
            var nextMonth = month.AddMonths(1);
            var end = new DateTime(nextMonth.Year, nextMonth.Month, 1).ToUniversalTime().ToString("yyyy-MM-ddThh:mm:ss");
            var url = "https://graph.microsoft.com/v1.0/me/calendars/" + calendar.Id + "/events?$filter=start/dateTime ge \'" + start + "\' and start/dateTime lt \'" + end + "\'";

            var events = await GetAsyncWithType<Models.Microsoft.EventList>(url);
            foreach (var obj in events.value) { obj.OverrideColor = calendar.Color; }
            return events;
        }
    }
}

using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.Google;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class GoogleClient : HTTPClientBase
    {
        string userInfoEndpoint = "https://www.googleapis.com/oauth2/v3/userinfo";
        string calendarEndpoint = "https://www.googleapis.com/calendar/v3";

        private UserBase user;

        /// <summary>
        /// This class enables you to retrieve data from Google APIs.
        /// </summary>
        /// <param name="token">OAuth2 token for Google API.</param>
        public GoogleClient(TokenBase token)
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

        public GoogleClient(UserBase user)
        {
            this.user = user;
            this.token = user.Token;
        }

        public async Task<GoogleUser> GetUserDataAsync()
        {
            var jsonString = await GetAsync(userInfoEndpoint);
            var userData = JsonSerializer.Deserialize<GoogleUser>(jsonString);
            userData.UserType = UserType.Google;
            userData.Token = token;

            this.user = userData;

            return userData;
        }

        public async Task<CalendarList> GetCalendarList()
        {
            var url = calendarEndpoint + "/users/me/calendarList";
            var calendars = await GetAsyncWithType<CalendarList>(url);
            foreach (var calendar in calendars.items)
            {
                calendar.UserId = user.Id;
                calendar.AccountType = UserType.Google.ToString();
            }

            return calendars;
        }

        public async Task<EventList> GetEventList(string id)
        {
            var url = calendarEndpoint + "/calendars/" + id + "/events";
            var jsonString = await GetAsync(url);
            if (string.IsNullOrEmpty(jsonString))
            {
                return null;
            }
            else
            {
                return JsonSerializer.Deserialize<EventList>(jsonString);
            }
        }

        public async Task<EventList> GetEventList(CalendarBase calendar, DateTime month)
        {
            var url = calendarEndpoint + "/calendars/" + calendar.Id + "/events?timeMin=" + new DateTime(month.Year, month.Month, 1).ToString("yyyy-MM-ddThh:mm:ssZ") + "&timeMax=" + new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month)).ToString("yyyy-MM-ddThh:mm:ssZ");
            var jsonString = await GetAsync(url);
            if (string.IsNullOrEmpty(jsonString))
            {
                return null;
            }
            else
            {
                var events = JsonSerializer.Deserialize<EventList>(jsonString);
                foreach (var obj in events.items) { obj.OverrideColor = calendar.OverrideColor; }
                return events;
            }
        }

        public async Task<Colors> GetColors()
        {
            var url = "https://www.googleapis.com/calendar/v3/colors";
            var jsonString = await GetAsync(url);
            return JsonSerializer.Deserialize<Colors>(jsonString);
        }
    }
}

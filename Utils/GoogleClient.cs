using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Auth;
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

        /// <summary>
        /// This class enables you to retrieve data from Google APIs.
        /// </summary>
        /// <param name="token">OAuth2 token for Google API.</param>
        public GoogleClient(TokenBase token)
        {
            this.token = token;
        }

        public async Task<GoogleUser> GetUserDataAsync()
        {
            var jsonString = await GetAsync(userInfoEndpoint);
            var userData = JsonSerializer.Deserialize<GoogleUser>(jsonString);
            userData.UserType = UserType.Google;
            userData.Token = token;
            return userData;
        }

        public async Task<CalendarList> GetCalendarList()
        {
            var url = calendarEndpoint + "/users/me/calendarList";
            var jsonString = await GetAsync(url);
            return JsonSerializer.Deserialize<CalendarList>(jsonString);
        }

        public async Task<EventList> GetEventList(Calendar calendar)
        {
            var url = calendarEndpoint + "/calendars/" + calendar.id + "/events";
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

        public async Task<EventList> GetEventList(Calendar calendar, DateTime month)
        {
            var url = calendarEndpoint + "/calendars/" + calendar.id + "/events?timeMin=" + new DateTime(month.Year, month.Month, 1).ToString("yyyy-MM-ddThh:mm:ssZ") + "&timeMax=" + new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month)).ToString("yyyy-MM-ddThh:mm:ssZ");
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

        public async Task<Colors> GetColors()
        {
            var url = "https://www.googleapis.com/calendar/v3/colors";
            var jsonString = await GetAsync(url);
            return JsonSerializer.Deserialize<Colors>(jsonString);
        }
    }
}

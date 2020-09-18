using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Google;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.ViewModels
{
    public class DashboardPageViewModel : ViewModelBase
    {
        private CalendarMonth _CalendarMonth;
        public CalendarMonth CalendarMonth
        {
            get { return _CalendarMonth; }
            set
            {
                _CalendarMonth = value;
                RaisePropertyChanged();
            }
        }

        public async void Init()
        {
            IsLoading = true;
            if (AppGlobalVariables.Users.Count > 0)
            {
                var events = new List<Event>();
                foreach (var user in AppGlobalVariables.Users)
                {
                    if (user.UserType == Models.Auth.UserType.Google)
                    {
                        var googleClient = new GoogleClient(user.Token);
                        var calendars = await googleClient.GetCalendarList();
                        if (calendars.items.Count > 0)
                        {
                            foreach (var item in calendars.items)
                            {
                                var tmp = await googleClient.GetEventList(item, DateTime.Now);
                                if (tmp != null)
                                {
                                    events.AddRange(tmp.items);
                                }
                            }
                        }
                    }
                }

                CalendarMonth = new CalendarMonth(DateTime.Now, events);
            }
            IsLoading = false;
        }
    }
}

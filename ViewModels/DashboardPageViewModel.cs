using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Google;
using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

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

        private List<Device> _Devices;
        public List<Device> Devices
        {
            get { return _Devices; }
            set
            {
                _Devices = value;
                RaisePropertyChanged();
            }
        }

        public async void Init()
        {
            IsLoading = true;
            if (AppGlobalVariables.Users.Count > 0)
            {
                var events = new List<Event>();
                var devices = new List<Device>();
                foreach (var user in AppGlobalVariables.Users)
                {
                    try
                    {
                        if (user.UserType == Models.Auth.UserType.Google)
                        {
                            LoadingMessage = "Retrieving calendar events...";
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
                        else if (user.UserType == Models.Auth.UserType.NatureRemo)
                        {
                            LoadingMessage = "Retrieving home status...";
                            var remoClient = new NatureRemoClient(user.Token);
                            devices.AddRange(await remoClient.GetDevicesAsync());
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugHelper.WriteErrorLog("Error occured while " + LoadingMessage + " User=" + user.UserName, ex);
                        await new MessageDialog(ex.Message + ". User=" + user.UserName + ".", "Error occured while retrieving " + LoadingMessage).ShowAsync();
                    }
                }

                CalendarMonth = new CalendarMonth(DateTime.Now, events);
                Devices = devices;
            }
            IsLoading = false;
        }
    }
}

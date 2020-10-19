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
using Windows.UI.Xaml;

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

        private DateTime _SelectedMonth;
        public DateTime SelectedMonth
        {
            get { return _SelectedMonth; }
            set
            {
                _SelectedMonth = value;
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

        public async void Init(DateTime datetime)
        {
            IsLoading = true;
            if (AppGlobalVariables.Users.Count > 0)
            {
                SelectedMonth = datetime;

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

                            if (AppGlobalVariables.Colors == null)
                            {
                                AppGlobalVariables.Colors = await googleClient.GetColors();
                            }

                            var calendars = await googleClient.GetCalendarList();
                            if (calendars.items.Count > 0)
                            {
                                foreach (var item in calendars.items)
                                {
                                    var tmp = await googleClient.GetEventList(item, datetime);
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

                CalendarMonth = new CalendarMonth(datetime, events);
                Devices = devices;
            }
            IsLoading = false;
        }

        public void MonthBackButton_Clicked(object sender, RoutedEventArgs e)
        {
            SelectedMonth = SelectedMonth.AddMonths(-1);

            ChangeMonth(SelectedMonth);
        }
        public void MonthForwardButton_Clicked(object sender, RoutedEventArgs e)
        {
            SelectedMonth = SelectedMonth.AddMonths(1);

            ChangeMonth(SelectedMonth);
        }

        private DateTime lastMonthChange;
        private async void ChangeMonth(DateTime datetime)
        {
            lastMonthChange = DateTime.Now;
            await Task.Delay(1000);
            if (DateTime.Now - lastMonthChange > new TimeSpan(0, 0, 1))
            {
                Init(datetime);
            }
        }
    }
}

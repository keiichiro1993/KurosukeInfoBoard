using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Google;
using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Utils.DBHelpers;
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
                try
                {
                    SelectedMonth = datetime;

                    var calendarTasks = new List<Task<List<Models.Common.EventBase>>>();
                    var remoTasks = new List<Task<List<Device>>>();

                    LoadingMessage = "Retrieving calendar events and statuses...";

                    foreach (var user in AppGlobalVariables.Users)
                    {

                        if (user.UserType == Models.Auth.UserType.Google)
                        {
                            calendarTasks.Add(GetGoogleEvents(datetime, user));
                        }
                        else if (user.UserType == Models.Auth.UserType.Microsoft)
                        {

                            calendarTasks.Add(GetMicrosoftEvents(datetime, user));
                        }
                        else if (user.UserType == Models.Auth.UserType.NatureRemo)
                        {
                            remoTasks.Add(GetRemoDevices(user));
                        }

                    }

                    var eventList = new List<Models.Common.EventBase>();
                    var eventLists = await Task.WhenAll(calendarTasks);
                    foreach (var events in eventLists) { eventList.AddRange(events); }

                    var deviceList = new List<Device>();
                    var deviceLists = await Task.WhenAll(remoTasks);
                    foreach (var devices in deviceLists) { deviceList.AddRange(devices); }

                    CalendarMonth = new CalendarMonth(datetime, eventList);
                    Devices = deviceList;
                }
                catch (Exception ex)
                {
                    DebugHelper.WriteErrorLog("Error occured while " + LoadingMessage + ".", ex);
                    await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
                }
            }
            IsLoading = false;
        }

        private async Task<List<Device>> GetRemoDevices(Models.Auth.UserBase user)
        {
            var devices = new List<Device>();

            try
            {
                var remoClient = new NatureRemoClient(user.Token);
                devices.AddRange(await remoClient.GetDevicesAsync());
            }
            catch (Exception ex)
            {
                DebugHelper.WriteErrorLog("Error occured while retrieving Nature Remo status. User=" + user.UserName, ex);
                await new MessageDialog(ex.Message + ". User=" + user.UserName + ".", "Error occured while retrieving Nature Remo status.").ShowAsync();
            }

            return devices;
        }

        private async Task<List<Models.Common.EventBase>> GetMicrosoftEvents(DateTime datetime, Models.Auth.UserBase user)
        {
            var events = new List<Models.Common.EventBase>();
            var msClient = new MicrosoftClient(user);

            if (user.Calendars.Count == 0)
            {
                var calendars = await msClient.GetCalendarList();
                var calCache = new CalendarCacheHelper();
                await calCache.Init();
                foreach (var calendar in calendars.value) { calCache.CheckIfEnabled(calendar); }
                user.Calendars.AddRange(calendars.value);
            }

            if (user.Calendars.Count > 0)
            {
                foreach (var calendar in user.Calendars)
                {
                    if (calendar.IsEnabled)
                    {
                        try
                        {
                            var msevents = await msClient.GetEventList(calendar.Id, datetime);
                            events.AddRange(msevents.value);
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.WriteErrorLog("Error occured while retrieving Microsoft Events. User=" + user.UserName + ". Calendar=" + calendar.Name + ".", ex);
                            await new MessageDialog(ex.Message + ". User=" + user.UserName + ". Calendar=" + calendar.Name + ". Please consider excluding this calendar.", "Error occured while retrieving Microsoft Events.").ShowAsync();
                        }
                    }
                }
            }

            return events;
        }

        private async Task<List<Models.Common.EventBase>> GetGoogleEvents(DateTime datetime, Models.Auth.UserBase user)
        {
            var events = new List<Models.Common.EventBase>();
            var googleClient = new GoogleClient(user);

            if (AppGlobalVariables.Colors == null)
            {
                AppGlobalVariables.Colors = await googleClient.GetColors();
            }

            if (user.Calendars.Count == 0)
            {
                var calendars = await googleClient.GetCalendarList();

                var calCache = new CalendarCacheHelper();
                await calCache.Init();
                foreach (var calendar in calendars.items) { calCache.CheckIfEnabled(calendar); }
                user.Calendars.AddRange(calendars.items);
            }

            if (user.Calendars.Count > 0)
            {
                foreach (var item in user.Calendars)
                {
                    if (item.IsEnabled)
                    {
                        try
                        {
                            var tmp = await googleClient.GetEventList(item.Id, datetime);
                            if (tmp != null)
                            {
                                events.AddRange(tmp.items);
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugHelper.WriteErrorLog("Error occured while retrieving Google Events. User=" + user.UserName + ". Calendar=" + item.Name + ".", ex);
                            await new MessageDialog(ex.Message + ". User=" + user.UserName + ". Calendar=" + item.Name + ". Please consider excluding this calendar.", "Error occured while retrieving Google Events.").ShowAsync();
                        }
                    }
                }
            }

            return events;
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

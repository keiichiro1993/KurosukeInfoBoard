using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Google
{
    public class DefaultReminder
    {
        public string method { get; set; }
        public int minutes { get; set; }
    }

    public class Notification
    {
        public string type { get; set; }
        public string method { get; set; }
    }

    public class NotificationSettings
    {
        public List<Notification> notifications { get; set; }
    }

    public class ConferenceProperties
    {
        public List<string> allowedConferenceSolutionTypes { get; set; }
    }

    public class Calendar : Common.CalendarBase
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string timeZone { get; set; }
        public string summaryOverride { get; set; }
        public string colorId { get; set; }
        public string backgroundColor { get; set; }
        public string foregroundColor { get; set; }
        public bool hidden { get; set; }
        public bool selected { get; set; }
        public string accessRole { get; set; }
        public List<DefaultReminder> defaultReminders { get; set; }
        public NotificationSettings notificationSettings { get; set; }
        public bool primary { get; set; }
        public bool deleted { get; set; }
        public ConferenceProperties conferenceProperties { get; set; }

        public override string Id { get { return id; } }
        public override string Name { get { return summary; } }
        public override string Color
        {
            get
            {
                var property = AppGlobalVariables.Colors.@event.GetType().GetProperty("color" + colorId);
                if (property != null)
                {
                    return (property.GetValue(AppGlobalVariables.Colors.@event) as Color).background;
                }
                else
                {
                    return ((Windows.UI.Xaml.Media.SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources["ApplicationPageBackgroundThemeBrush"]).Color.ToString();
                }
            }
        }
    }

    public class CalendarList
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string nextSyncToken { get; set; }
        public List<Calendar> items { get; set; }
    }

}

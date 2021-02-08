using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Microsoft
{
    public class Body
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class Start
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class End
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class Location
    {
        public string displayName { get; set; }
        public string locationType { get; set; }
        public string uniqueId { get; set; }
        public string uniqueIdType { get; set; }
    }

    public class Status
    {
        public string response { get; set; }
        public DateTime time { get; set; }
    }

    public class EmailAddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class Attendee
    {
        public string type { get; set; }
        public Status status { get; set; }
        public EmailAddress emailAddress { get; set; }
    }

    public class Organizer
    {
        public EmailAddress emailAddress { get; set; }
    }

    public class ResponseStatus
    {
        public string response { get; set; }
        public DateTime time { get; set; }
    }

    public class Event : Common.EventBase
    {
        [JsonPropertyName("@odata.etag")]
        public string OdataEtag { get; set; }
        public string id { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string changeKey { get; set; }
        public List<object> categories { get; set; }
        public string transactionId { get; set; }
        public string originalStartTimeZone { get; set; }
        public string originalEndTimeZone { get; set; }
        public string iCalUId { get; set; }
        public int reminderMinutesBeforeStart { get; set; }
        public bool isReminderOn { get; set; }
        public bool hasAttachments { get; set; }
        public string subject { get; set; }
        public string bodyPreview { get; set; }
        public string importance { get; set; }
        public string sensitivity { get; set; }
        public bool isAllDay { get; set; }
        public bool isCancelled { get; set; }
        public bool isOrganizer { get; set; }
        public bool responseRequested { get; set; }
        public object seriesMasterId { get; set; }
        public string showAs { get; set; }
        public string type { get; set; }
        public string webLink { get; set; }
        public string onlineMeetingUrl { get; set; }
        public bool isOnlineMeeting { get; set; }
        public string onlineMeetingProvider { get; set; }
        public bool allowNewTimeProposals { get; set; }
        public bool isDraft { get; set; }
        public bool hideAttendees { get; set; }
        public ResponseStatus responseStatus { get; set; }
        public Body body { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
        public Location location { get; set; }
        public List<object> locations { get; set; }
        public object recurrence { get; set; }
        public List<Attendee> attendees { get; set; }
        public Organizer organizer { get; set; }
        public object onlineMeeting { get; set; }
        [JsonPropertyName("calendar@odata.associationLink")]
        public string CalendarOdataAssociationLink { get; set; }
        [JsonPropertyName("calendar@odata.navigationLink")]
        public string CalendarOdataNavigationLink { get; set; }


        //EventBase の実装
        public override string Subject { get { return subject; } }
        public override bool IsAllDay { get { return isAllDay; } }
        public override string Content { get { return body.contentType == "html" ? body.content : "<html>" + body.content.Replace("\n", "<br/>") + "</html>"; } }
        public override DateTime Start { get { return start.dateTime.ToLocalTime(); } }
        public override DateTime End { get { return end.dateTime.ToLocalTime(); } }
        public override string OverrideColor { get; set; }

        public override string EventColor { get { return !string.IsNullOrEmpty(OverrideColor) ? OverrideColor : ((Windows.UI.Xaml.Media.SolidColorBrush)Windows.UI.Xaml.Application.Current.Resources["ApplicationPageBackgroundThemeBrush"]).Color.ToString(); } }

        public override bool IsEventDateMatched(DateTime date)
        {
            return Start <= date.Date && date.Date < End;
        }
    }

    public class EventList
    {
        [JsonPropertyName("@odata.context")]
        public string OdataContext { get; set; }
        public List<Event> value { get; set; }
    }


}

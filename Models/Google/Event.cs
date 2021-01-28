using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Google
{
    public class Creator
    {
        public string id { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public bool self { get; set; }
    }

    public class Organizer
    {
        public string id { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public bool self { get; set; }
    }

    public class Start
    {
        public DateTime date { get; set; }
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class End
    {
        public DateTime date { get; set; }
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class OriginalStartTime
    {
        public DateTime date { get; set; }
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class Attendee
    {
        public string id { get; set; }
        public string email { get; set; }
        public string displayName { get; set; }
        public bool organizer { get; set; }
        public bool self { get; set; }
        public bool resource { get; set; }
        public bool optional { get; set; }
        public string responseStatus { get; set; }
        public string comment { get; set; }
        public int additionalGuests { get; set; }
    }

    public class ExtendedProperties
    {
        [JsonPropertyName("private")]
        public object privateObject { get; set; }
        public object shared { get; set; }
    }

    public class ConferenceSolutionKey
    {
        public string type { get; set; }
    }

    public class Status
    {
        public string statusCode { get; set; }
    }

    public class CreateRequest
    {
        public string requestId { get; set; }
        public ConferenceSolutionKey conferenceSolutionKey { get; set; }
        public Status status { get; set; }
    }

    public class EntryPoint
    {
        public string entryPointType { get; set; }
        public string uri { get; set; }
        public string label { get; set; }
        public string pin { get; set; }
        public string accessCode { get; set; }
        public string meetingCode { get; set; }
        public string passcode { get; set; }
        public string password { get; set; }
    }

    public class Key
    {
        public string type { get; set; }
    }

    public class ConferenceSolution
    {
        public Key key { get; set; }
        public string name { get; set; }
        public string iconUri { get; set; }
    }

    public class ConferenceData
    {
        public CreateRequest createRequest { get; set; }
        public List<EntryPoint> entryPoints { get; set; }
        public ConferenceSolution conferenceSolution { get; set; }
        public string conferenceId { get; set; }
        public string signature { get; set; }
        public string notes { get; set; }
    }

    public class Preferences
    {
        public string test { get; set; }
    }

    public class Gadget
    {
        public string type { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string iconLink { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string display { get; set; }
        public Preferences preferences { get; set; }
    }

    public class Override
    {
        public string method { get; set; }
        public int minutes { get; set; }
    }

    public class Reminders
    {
        public bool useDefault { get; set; }
        public List<Override> overrides { get; set; }
    }

    public class Source
    {
        public string url { get; set; }
        public string title { get; set; }
    }

    public class Attachment
    {
        public string fileUrl { get; set; }
        public string title { get; set; }
        public string mimeType { get; set; }
        public string iconLink { get; set; }
        public string fileId { get; set; }
    }

    public class Event : Common.EventBase
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string id { get; set; }
        public string status { get; set; }
        public string htmlLink { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string colorId { get; set; }
        public Creator creator { get; set; }
        public Organizer organizer { get; set; }
        public Start start { get; set; }
        public End end { get; set; }
        public bool endTimeUnspecified { get; set; }
        public List<string> recurrence { get; set; }
        public string recurringEventId { get; set; }
        public OriginalStartTime originalStartTime { get; set; }
        public string transparency { get; set; }
        public string visibility { get; set; }
        public string iCalUID { get; set; }
        public int sequence { get; set; }
        public List<Attendee> attendees { get; set; }
        public bool attendeesOmitted { get; set; }
        public ExtendedProperties extendedProperties { get; set; }
        public string hangoutLink { get; set; }
        public ConferenceData conferenceData { get; set; }
        public Gadget gadget { get; set; }
        public bool anyoneCanAddSelf { get; set; }
        public bool guestsCanInviteOthers { get; set; }
        public bool guestsCanModify { get; set; }
        public bool guestsCanSeeOtherGuests { get; set; }
        public bool privateCopy { get; set; }
        public bool locked { get; set; }
        public Reminders reminders { get; set; }
        public Source source { get; set; }
        public List<Attachment> attachments { get; set; }

        //EventBase の実装
        public override string Subject { get { return summary; } }
        public override string Content { get { return description; } }
        public override DateTime Start { get { return start.date == new DateTime(1, 1, 1) ? start.dateTime : start.date; } }
        public override DateTime End { get { return end.date == new DateTime(1, 1, 1) ? end.dateTime : end.date; } }
        public override bool IsAllDay { get { return start.date != new DateTime(1, 1, 1) && end.date != new DateTime(1, 1, 1); } }
        public override string EventColor
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
        public override bool IsEventDateMatched(DateTime date)
        {
            if (start == null)
            {
                return false;
            }

            return (Start <= date && End > date) || (Start.Date == date.Date);
        }
    }

    public class EventList
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string summary { get; set; }
        public string description { get; set; }
        public DateTime updated { get; set; }
        public string timeZone { get; set; }
        public string accessRole { get; set; }
        public List<DefaultReminder> defaultReminders { get; set; }
        public string nextPageToken { get; set; }
        public string nextSyncToken { get; set; }
        public List<Event> items { get; set; }
    }


}

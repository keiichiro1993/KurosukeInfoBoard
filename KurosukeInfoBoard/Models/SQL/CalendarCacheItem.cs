namespace KurosukeInfoBoard.Models.SQL
{
    public class CalendarCacheEntity
    {
        public long ID { get; set; }

        public string CalendarId { get; set; }

        public string CalendarName { get; set; }

        public bool IsEnabled { get; set; }

        public string AccountType { get; set; }
        
        public string UserId { get; set; }

        public string OverrideColor { get; set; }
    }
}

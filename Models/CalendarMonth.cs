using KurosukeInfoBoard.Models.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models
{
    public class CalendarMonth
    {
        public List<CalendarDay> CalendarDays { get; set; } = new List<CalendarDay>();
        public DateTime Month { get; set; }

        public CalendarMonth(DateTime month, List<Common.EventBase> events)
        {
            Month = month;
            var days = DateTime.DaysInMonth(month.Year, month.Month);
            for (int i = 1; i <= days; i++)
            {
                CalendarDays.Add(new CalendarDay(new DateTime(month.Year, month.Month, i)));
            }

            if (events != null && events.Count > 0)
            {
                foreach (var day in CalendarDays)
                {
                    var items = from item in events
                                where item.IsEventDateMatched((DateTime)day.Date)
                                select item;
                    day.Events.AddRange(items);
                }
            }

            var dayOfTheWeek = new DateTime(month.Year, month.Month, 1).DayOfWeek;
            var blankDaysToAdd = (int)dayOfTheWeek;
            if (blankDaysToAdd > 0)
            {
                for (var i = 0; i < blankDaysToAdd; i++)
                {
                    CalendarDays.Insert(0, new CalendarDay());
                }
            }

            while (CalendarDays.Count < 42)
            {
                CalendarDays.Add(new CalendarDay());
            }
        }
    }
}

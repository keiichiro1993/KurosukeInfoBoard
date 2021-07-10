using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models
{
    public class CalendarDay
    {
        public bool IsSpacer { get; set; }
        public DateTime Date { get; set; }
        public List<Common.EventBase> Events { get; set; } = new List<Common.EventBase>();

        public CalendarDay(DateTime date)
        {
            Date = date;
        }

        public CalendarDay() { }

    }
}
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
        public List<Google.Event> Events { get; set; } = new List<Google.Event>();

        public CalendarDay(DateTime date)
        {
            Date = date;
        }

        public CalendarDay() { }

    }
}
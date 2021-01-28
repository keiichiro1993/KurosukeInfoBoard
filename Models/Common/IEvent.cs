using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Common
{
    public abstract class EventBase
    {
        public abstract string Subject { get; }
        public abstract string Content { get; }
        public abstract DateTime Start { get; }
        public abstract DateTime End { get; }
        public abstract string EventColor { get; }
        public abstract bool IsAllDay { get; }

        public abstract bool IsEventDateMatched(DateTime date);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Common
{
    public abstract class CalendarBase
    {
        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract bool IsEnabled { get; set; }
        public abstract string Color { get; set; } //hex
        public abstract string OverrideColor { get; set; }

        public abstract string AccountType { get; set; }
        public abstract string UserId { get; set; }
    }
}

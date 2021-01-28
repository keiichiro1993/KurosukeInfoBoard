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
        public abstract string Color { get; } //hex
    }
}

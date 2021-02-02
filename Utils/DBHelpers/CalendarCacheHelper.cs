using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public class CalendarCacheHelper
    {
        public async Task Init()
        {
            using (var context = new CalendarCacheContext())
            {
                await context.Database.EnsureCreatedAsync();
            }
        }

        private CalendarCacheEntity ICalendarToEntity(CalendarBase calendar)
        {
            var entity = new CalendarCacheEntity();
            entity.CalendarId = calendar.Id;
            entity.CalendarName = calendar.Name;
            entity.IsEnabled = calendar.IsEnabled;
            entity.AccountType = calendar.AccountType;
            entity.UserId = calendar.UserId;

            return entity;
        }

        public async Task SaveCalendar(CalendarBase calendar)
        {
            var entity = ICalendarToEntity(calendar);
            using (var context = new CalendarCacheContext())
            {
                await context.CalendarCache.AddAsync(entity);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// This method checks if the calendar item is enabled for display.
        /// Add calendar entity to DB if the item doesn't exist.
        /// </summary>
        /// <param name="calendar">the calendar item to check</param>
        /// <returns>True if it doens't exist on cache or exists and enabled. False if disabled.</returns>
        public bool CheckIfEnabled(CalendarBase calendar)
        {
            using (var context = new CalendarCacheContext())
            {
                var match = from item in context.CalendarCache
                            where item.CalendarId == calendar.Id && item.UserId == calendar.UserId
                            select item;
                if (match.Any())
                {
                    calendar.IsEnabled = match.First().IsEnabled;
                    return match.First().IsEnabled;
                }
            }
            calendar.IsEnabled = true;
            return true;
        }

        /// <summary>
        /// Add or Update calendar.
        /// </summary>
        /// <param name="calendar">The calaendar item to Add or Update</param>
        /// <returns></returns>
        public async Task AddUpdateCalendarCache(CalendarBase calendar)
        {
            using (var context = new CalendarCacheContext())
            {
                var match = from item in context.CalendarCache
                            where item.CalendarId == calendar.Id && item.UserId == calendar.UserId
                            select item;

                if (match.Any())
                {
                    var entity = match.First();
                    entity.IsEnabled = calendar.IsEnabled;
                    context.Update(entity);

                    await context.SaveChangesAsync();
                    return;
                }
            }

            await SaveCalendar(calendar);
        }

    }
}

using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public class CalendarCacheHelper
    {
        private static Task AwaitingWriteTask;
        private string dbFileName = "calendarcache.json";
        private StorageFile dbFile;
        private List<CalendarCacheEntity> dbItems;
        public async Task Init()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (!await localFolder.FileExists(dbFileName))
            {
                //create file if not exist
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                dbItems = new List<CalendarCacheEntity>();
                await SaveFile();
            }
            else
            {
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                var json = await FileIO.ReadTextAsync(dbFile);
                dbItems = JsonConvert.DeserializeObject<List<CalendarCacheEntity>>(json);
            }
        }

        /// <summary>
        /// This method checks if the calendar item is enabled for display.
        /// Add calendar entity to DB if the item doesn't exist.
        /// </summary>
        /// <param name="calendar">the calendar item to check</param>
        /// <returns>True if it doens't exist on cache or exists and enabled. False if disabled.</returns>
        public async Task<bool> CheckIfEnabled(CalendarBase calendar)
        {
            if (dbItems == null) { await Init(); }
            var match = (from item in dbItems
                         where item.CalendarId == calendar.Id && item.UserId == calendar.UserId
                         select item).FirstOrDefault();
            if (match != null)
            {
                calendar.IsEnabled = match.IsEnabled;
                calendar.OverrideColor = match.OverrideColor;
                return match.IsEnabled;
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
            await waitPreviousTask();
            AwaitingWriteTask = AddUpdateCalendarCacheInternal(calendar);
            await AwaitingWriteTask;
        }

        private async Task AddUpdateCalendarCacheInternal(CalendarBase calendar)
        {
            // update existing item
            if (dbItems == null) { await Init(); }
            var match = (from item in dbItems
                         where item.CalendarId == calendar.Id && item.UserId == calendar.UserId
                         select item).FirstOrDefault();
            if (match != null)
            {
                match.IsEnabled = calendar.IsEnabled;
                match.OverrideColor = calendar.OverrideColor;
            }
            else
            {
                dbItems.Add(ICalendarToEntity(calendar));
            }

            await SaveFile();
        }

        private async Task waitPreviousTask()
        {
            if (AwaitingWriteTask != null)
            {
                while (!AwaitingWriteTask.IsCompleted)
                {
                    await Task.Delay(100);
                }
            }
        }

        private async Task SaveFile()
        {
            var json = JsonConvert.SerializeObject(dbItems);
            await FileIO.WriteTextAsync(dbFile, json);
        }

        private CalendarCacheEntity ICalendarToEntity(CalendarBase calendar)
        {
            var entity = new CalendarCacheEntity();
            entity.CalendarId = calendar.Id;
            entity.CalendarName = calendar.Name;
            entity.IsEnabled = calendar.IsEnabled;
            entity.AccountType = calendar.AccountType;
            entity.UserId = calendar.UserId;
            entity.OverrideColor = calendar.OverrideColor;

            return entity;
        }
    }
}

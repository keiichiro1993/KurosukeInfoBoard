using KurosukeInfoBoard.Models.SQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public class CombinedControlHelper
    {
        private static Task AwaitingWriteTask;
        private string dbFileName = "combinedcontrols.json";
        private StorageFile dbFile;
        private List<CombinedControl> dbItems;

        public async Task Init()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (!await localFolder.FileExists(dbFileName))
            {
                //create file if not exist
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                dbItems = new List<CombinedControl>();
                await SaveFile();
            }
            else
            {
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                var json = await FileIO.ReadTextAsync(dbFile);
                dbItems = JsonConvert.DeserializeObject<List<CombinedControl>>(json);
            }
        }

        private async Task SaveFile()
        {
            var json = JsonConvert.SerializeObject(dbItems);
            await FileIO.WriteTextAsync(dbFile, json);
        }

        public async Task<ObservableCollection<CombinedControl>> GetCombinedControls()
        {
            if (dbItems == null) { await Init(); }
            return new ObservableCollection<CombinedControl>(from item in dbItems
                                                             orderby item.Order
                                                             select item);
        }

        /// <summary>
        /// Add or Update Combined Control setting item
        /// </summary>
        /// <param name="combinedControl">Combined Control item newly created or retrieved from DB and updated.</param>
        /// <returns></returns
        public async Task AddUpdateCombinedControl(CombinedControl combinedControl)
        {
            await waitPreviousTask();
            AwaitingWriteTask = AddUpdateCombinedControlInternal(combinedControl);
            await AwaitingWriteTask;
        }
        private async Task AddUpdateCombinedControlInternal(CombinedControl combinedControl)
        {
            if (combinedControl.Id == null)
            {
                // add new item
                Int64 id = 0;
                var latestItem = (from item in dbItems
                                  orderby item.Id descending
                                  select item).FirstOrDefault();
                if (latestItem != null) { id = (Int64)latestItem.Id + 1; }
                combinedControl.Id = id;
                combinedControl.Order = id;
                dbItems.Add(combinedControl);
            }
            else
            {
                // update existing item
                var match = (from item in dbItems
                             where item.Id == combinedControl.Id
                             select item).FirstOrDefault();
                if (match != null)
                {
                    var index = dbItems.IndexOf(match);
                    dbItems[index] = combinedControl;
                }
                else
                {
                    dbItems.Add(combinedControl);
                }
            }

            await SaveFile();
        }

        public async Task RemoveCombindControl(CombinedControl combinedControl)
        {
            await waitPreviousTask();
            AwaitingWriteTask = RemoveCombindControlInternal(combinedControl);
            await AwaitingWriteTask;
        }

        private async Task RemoveCombindControlInternal(CombinedControl combinedControl)
        {
            var match = (from item in dbItems
                         where item.Id == combinedControl.Id
                         select item).FirstOrDefault();
            if (match != null)
            {
                dbItems.Remove(match);
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
    }
}

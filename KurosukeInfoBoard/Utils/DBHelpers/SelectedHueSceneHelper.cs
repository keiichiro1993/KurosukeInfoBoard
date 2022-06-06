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
    public class SelectedHueSceneHelper
    {
        private static Task AwaitingWriteTask;
        private string dbFileName = "hueselectedscenes.json";
        private StorageFile dbFile;
        private List<HueSelectedSceneEntity> dbItems;

        public async Task Init()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (!await localFolder.FileExists(dbFileName))
            {
                //create file if not exist
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                dbItems = new List<HueSelectedSceneEntity>();
                await SaveFile();
            }
            else
            {
                dbFile = await localFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
                var json = await FileIO.ReadTextAsync(dbFile);
                dbItems = JsonConvert.DeserializeObject<List<HueSelectedSceneEntity>>(json);
            }
        }

        /// <summary>
        /// Add or Update Combined Control setting item
        /// </summary>
        /// <param name="combinedControl">Combined Control item newly created or retrieved from DB and updated.</param>
        /// <returns></returns
        public async Task AddUpdateSelectedHueScene(HueSelectedSceneEntity selectedHueScene)
        {
            await waitPreviousTask();
            AwaitingWriteTask = AddUpdateSelectedHueSceneInternal(selectedHueScene);
            await AwaitingWriteTask;
        }
        private async Task AddUpdateSelectedHueSceneInternal(HueSelectedSceneEntity selectedHueScene)
        {
            await RemoveSelectedHueSceneInternal(selectedHueScene);
            dbItems.Add(selectedHueScene);
            await SaveFile();
        }

        public async Task<HueSelectedSceneEntity> GetExistingSceneEntity(string bridgeId, string groupId)
        {
            if (dbItems == null) { await Init(); }
            return (from item in dbItems
                    where item.HueId == bridgeId && item.RoomId == groupId
                    select item).FirstOrDefault();
        }

        public async Task RemoveSelectedHueScene(HueSelectedSceneEntity selectedHueScene)
        {
            await waitPreviousTask();
            AwaitingWriteTask = RemoveSelectedHueSceneInternal(selectedHueScene);
            await AwaitingWriteTask;
        }
        private async Task RemoveSelectedHueSceneInternal(HueSelectedSceneEntity selectedHueScene)
        {
            if (dbItems == null) { await Init(); }
            // update existing item
            var match = (from item in dbItems
                         where item.HueId == selectedHueScene.HueId && item.RoomId == selectedHueScene.RoomId
                         select item).FirstOrDefault();
            if (match != null)
            {
                dbItems.Remove(match);
            }
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
    }
}

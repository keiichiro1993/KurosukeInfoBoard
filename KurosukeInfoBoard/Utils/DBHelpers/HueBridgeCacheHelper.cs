using KurosukeInfoBoard.Models.SQL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public class HueBridgeCacheHelper : DBHelperBase
    {
        string fileName = "huebridge.db";
        HueBridgeCacheEntity dbContent;

        private async Task Init()
        {
            if (assetDBFile == null)
            {
                assetDBFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(".assetdb", CreationCollisionOption.OpenIfExists);
                if (!await assetDBFolder.FileExists(fileName))
                {
                    assetDBFile = await assetDBFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                    dbContent = new HueBridgeCacheEntity(new List<HueBridgeCacheItem>());
                    await SaveObjectToJsonFile(dbContent);
                }
                else
                {
                    assetDBFile = await assetDBFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                    var jsonString = await FileIO.ReadTextAsync(assetDBFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    dbContent = JsonConvert.DeserializeObject<HueBridgeCacheEntity>(jsonString);
                    // workaround for JsonConvert returning null instead of empty array
                    if (dbContent.HueBridgeCache == null)
                    {
                        dbContent.HueBridgeCache = new List<HueBridgeCacheItem>();
                    }
                }
            }
        }

        public async Task<string> GetHueBridgeCachedIp(string bridgeId)
        {
            await Init();

            var hit = from item in dbContent.HueBridgeCache
                      where item.Id == bridgeId
                      select item;
            if (hit.Any())
            {
                return hit.First().IpAddress;
            }
            else
            {
                return null;
            }
        }

        public async Task SaveHueBridgeCache(string bridgeId, string ipAddress)
        {
            await Init();
            var hit = from item in dbContent.HueBridgeCache
                      where item.Id == bridgeId
                      select item;
            if (hit.Any())
            {
                hit.First().IpAddress = ipAddress;
            }
            else
            {
                dbContent.HueBridgeCache.Add(new HueBridgeCacheItem(bridgeId, ipAddress));
            }
            await SaveObjectToJsonFile(dbContent);
        }
    }
}

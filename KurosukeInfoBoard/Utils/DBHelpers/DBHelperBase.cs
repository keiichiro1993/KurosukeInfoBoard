using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Windows.Storage;


namespace KurosukeInfoBoard.Utils.DBHelpers
{
    public abstract class DBHelperBase
    {
        private protected StorageFile assetDBFile;
        private protected StorageFolder assetDBFolder;
        private protected async Task SaveObjectToJsonFile<T>(T target, int retry = 3, bool isRetry = false)
        {
            if (assetDBFile == null)
            {
                throw new InvalidOperationException("DB Asset File not correctly set. The DB helper should be initialized before the save operation.");
            }

            // backup
            var backupName = assetDBFile.Name + ".backup";
            if (!isRetry)
            {
                await assetDBFile.CopyAsync(assetDBFolder, backupName, NameCollisionOption.ReplaceExisting);
            }

            // save
            await assetDBFile.DeleteAsync();
            assetDBFile = await assetDBFolder.CreateFileAsync(assetDBFile.Name, CreationCollisionOption.OpenIfExists);
            var jsonString = JsonConvert.SerializeObject(target);
            await FileIO.WriteTextAsync(assetDBFile, jsonString, Windows.Storage.Streams.UnicodeEncoding.Utf8);

            // validate if the save operation succeeded
            try
            {
                jsonString = await FileIO.ReadTextAsync(assetDBFile, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                retry--;
                if (retry > 0)
                {
                    await SaveObjectToJsonFile(target, retry, true);
                }
                else
                {
                    throw new Exception($"Validation of saved file failed. The previous content is available as {backupName}", ex);
                }
            }
        }
    }
}

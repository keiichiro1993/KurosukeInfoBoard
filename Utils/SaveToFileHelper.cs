using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KurosukeInfoBoard.Utils
{
    public static class SaveToFileHelper
    {
        public static async Task SaveStringToFileWithFilePicker(string savedString, string extension, Encoding encoding)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add(extension.ToUpper(), new List<string>() { "." + extension.ToLower() });
            savePicker.SuggestedFileName = DateTime.Now.ToLongDateString();

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                //Encoding
                var bytes = encoding.GetBytes(savedString);

                Windows.Storage.CachedFileManager.DeferUpdates(file);
                await Windows.Storage.FileIO.WriteBytesAsync(file, bytes);

                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
            }
        }

        public static async Task SaveStringToAppLocalFile(string fileName, string content)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);

            await FileIO.WriteTextAsync(file, content, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        }

        public static async Task<string> ReadStringFromAppLocalFile(string fileName)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            var file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

            return await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
        }
    }
}

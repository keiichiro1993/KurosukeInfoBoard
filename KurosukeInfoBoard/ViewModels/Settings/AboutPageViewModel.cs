using DebugHelper;
using KurosukeInfoBoard.Utils;
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class AboutPageViewModel : ViewModelBase
    {
        private string _LicenseMD;
        public string LicenseMD
        {
            get { return _LicenseMD; }
            set
            {
                _LicenseMD = value;
                RaisePropertyChanged();
            }
        }

        private string _ReleaseMD;
        public string ReleaseMD
        {
            get { return _ReleaseMD; }
            set
            {
                _ReleaseMD = value;
                RaisePropertyChanged();
            }
        }

        public string AppVersion { get { return GetAppVersion(); } }

        private string GetAppVersion()
        {
            var version = Package.Current.Id.Version;
            return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
        }

        public string UserID { get { return SettingsHelper.CreateOrLoadGUID(); } }

        public async Task Init()
        {
            IsLoading = true;
            LoadingMessage = "Loading Mark Down texts...";

            try
            {
                StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                StorageFile file = await InstallationFolder.GetFileAsync("Assets\\TextResources\\LicenseCredit.md");
                LicenseMD = await FileIO.ReadTextAsync(file);

                file = await InstallationFolder.GetFileAsync("Assets\\TextResources\\ReleaseHistory.md");
                ReleaseMD = await FileIO.ReadTextAsync(file);
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while " + LoadingMessage + ".", ex);
                await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
            }

            IsLoading = false;
        }

        public async void DumpSettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            var json = SettingsHelper.DumpSettingsToJson();
            await SaveToFileHelper.SaveStringToFileWithFilePicker(json, "json", Encoding.UTF8);
        }
    }
}

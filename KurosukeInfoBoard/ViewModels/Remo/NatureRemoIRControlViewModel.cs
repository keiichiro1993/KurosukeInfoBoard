using DebugHelper;
using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels.Remo
{
    public class NatureRemoIRControlViewModel : Common.ViewModels.ViewModelBase
    {
        private Appliance _Appliance;
        public Appliance Appliance
        {
            get { return _Appliance; }
            set
            {
                _Appliance = value;
                RaisePropertyChanged();
            }
        }

        public void Init(Appliance appliance)
        {
            this.Appliance = appliance;
        }

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadingMessage = "Sending signal...";
            IsLoading = true;
            var button = (FrameworkElement)sender;
            var signal = (Signal)button.DataContext;

            try
            {
                var client = new NatureRemoClient(Appliance.Token);
                await client.PostSignal(signal.id);
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while " + LoadingMessage, ex);
                await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
            }

            IsLoading = false;
        }
    }
}

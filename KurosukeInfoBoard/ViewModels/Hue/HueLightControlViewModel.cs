using Common.ViewModels;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.ViewModels.Hue
{
    public class HueLightControlViewModel : ViewModelBase
    {
        private Models.Hue.Light _Light;
        public Models.Hue.Light Light
        {
            get { return _Light; }
            set
            {
                _Light = value;
                RaisePropertyChanged();
            }
        }

        public void Init(Models.Hue.Light light)
        {
            Light = light;
        }

        public bool IsOn
        {
            get { return Light.HueLight.State.On; }
            set
            {
                Light.HueLight.State.On = value;
                SendCommand();
            }
        }

        public byte Brightness
        {
            get { return Light.HueLight.State.Brightness; }
            set
            {
                Light.HueLight.State.Brightness = value;
                SendCommand();
            }
        }

        private DateTime lastCommand;
        private async void SendCommand()
        {
            lastCommand = DateTime.Now;
            await Task.Delay(1000);
            if (DateTime.Now - lastCommand >= new TimeSpan(0, 0, 1))
            {
                IsLoading = true;
                var client = new HueClient(Light.HueUser);
                await client.SendCommandAsync(Light.HueLight);
                IsLoading = false;
            }
        }
    }
}

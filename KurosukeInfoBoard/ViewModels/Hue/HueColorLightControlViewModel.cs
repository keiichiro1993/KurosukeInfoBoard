using Common.ViewModels;
using KurosukeInfoBoard.Utils;
using KurosukeInfoBoard.Utils.UIHelper;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace KurosukeInfoBoard.ViewModels.Hue
{
    public class HueColorLightControlViewModel : ViewModelBase
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
        private async void SendCommand(RGBColor? color = null)
        {
            lastCommand = DateTime.Now;
            await Task.Delay(1000);
            if (DateTime.Now - lastCommand >= new TimeSpan(0, 0, 1))
            {
                IsLoading = true;
                var client = new HueClient(Light.HueUser);
                Light.HueLight = await client.SendCommandAsync(Light.HueLight, color);
                RaisePropertyChanged("Light");
                RaisePropertyChanged("ColorBrush");
                IsLoading = false;
            }
        }

        public Color Color
        {
            get
            {
                var hex = Light.HueLight.State.ToHex();
                return ColorConverter.HexToColor("#" + hex);
            }
            set
            {
                var rgbColor = new RGBColor(BitConverter.ToString(new byte[] { value.R, value.G, value.B }).Replace("-", ""));
                SendCommand(rgbColor);
            }
        }

        public SolidColorBrush ColorBrush
        {
            get { return new SolidColorBrush(Color); }
        }
    }
}

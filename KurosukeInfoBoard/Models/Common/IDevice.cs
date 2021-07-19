using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Models.Common
{
    public interface IDevice
    {
        string DeviceName { get; }
        string RoomTemperature { get; }
        string RoomTemperatureUnit { get; }
        List<IAppliance> Appliances { get; set; }
        Visibility HeaderControlVisibility { get; set; }
        Visibility HeaderTemperatureVisibility { get; set; }

        byte HueBrightness { get; set; }
        bool HueIsOn { get; set; }
    }
}

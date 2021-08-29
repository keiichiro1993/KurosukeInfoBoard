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

        #region Hue
        Visibility HeaderControlVisibility { get; }
        Visibility HeaderTemperatureVisibility { get; }
        Visibility HeaderSceneControlVisibility { get; }
        List<Q42.HueApi.Models.Scene> HueScenes { get; set; }

        byte HueBrightness { get; set; }
        bool HueIsOn { get; set; }
        #endregion
    }
}

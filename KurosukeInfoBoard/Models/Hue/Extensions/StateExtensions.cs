using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Hue.Extensions
{
    public static class StateExtensions
    {
        public static bool CheckEquals(this State state, JsonState other)
        {
            return state.On == other.On &&
                   state.Alert == other.Alert &&
                   (state.ColorMode == "hsb" ? state.Brightness == other.Brightness : true) &&
                   (state.ColorMode == "xy" ? Math.Abs((int)(state.ColorCoordinates[0] * 10000) - other.ColorCoordinates[0]) <= 10 &&
                                              Math.Abs((int)(state.ColorCoordinates[1] * 10000) - other.ColorCoordinates[1]) <= 10 : true) &&
                   state.ColorMode == other.ColorMode &&
                   (state.ColorMode == "ct" ? state.ColorTemperature == other.ColorTemperature : true) &&
                   state.Effect == other.Effect &&
                   state.Mode == other.Mode &&
                   (state.ColorMode == "hsb" ? state.Saturation == other.Saturation : true) &&
                   state.TransitionTime == other.TransitionTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Common
{
    public interface IDevice
    {
        string DeviceName { get; }
        string RoomTemperature { get; }
        string RoomTemperatureUnit { get; }
        List<IAppliance> Appliances { get; set; }
    }
}

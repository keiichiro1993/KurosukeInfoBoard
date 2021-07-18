using KurosukeInfoBoard.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Hue
{
    public class Group : IDevice
    {
        public Group(Q42.HueApi.Models.Groups.Group group)
        {
            DeviceName = group.Name + " - " + group.Class;
            HueGroup = group;
        }

        Q42.HueApi.Models.Groups.Group HueGroup { get; set; }

        public string DeviceName { get; set; }

        public string RoomTemperature { get; set; } = "";

        public string RoomTemperatureUnit { get; set; } = "";

        public List<IAppliance> Appliances { get; set; }
    }
}

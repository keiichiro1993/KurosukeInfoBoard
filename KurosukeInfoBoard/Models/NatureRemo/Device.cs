using KurosukeInfoBoard.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.NatureRemo
{
    public class User
    {
        public string id { get; set; }
        public string nickname { get; set; }
        public bool superuser { get; set; }
    }

    public class NewestEvent
    {
        public double val { get; set; }
        public DateTime created_at { get; set; }
    }

    public class NewestEvents
    {
        public NewestEvent te { get; set; }
        public NewestEvent hu { get; set; }
        public NewestEvent il { get; set; }
        public NewestEvent mo { get; set; }
    }

    public class Device : IDevice
    {

        #region implementation of IDevice
        public string DeviceName { get { return name; } }
        public string RoomTemperature { get { return newest_events.te.val.ToString(); } }
        public string RoomTemperatureUnit { get { return string.IsNullOrEmpty(RoomTemperature) ? "" : "℃"; } }
        public List<IAppliance> Appliances { get; set; }
        #endregion

        public string name { get; set; }
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string mac_address { get; set; }
        public string serial_number { get; set; }
        public string firmware_version { get; set; }
        public int temperature_offset { get; set; }
        public int humidity_offset { get; set; }
        public List<User> users { get; set; }
        public NewestEvents newest_events { get; set; }

    }
}

﻿using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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

        public Visibility HeaderControlVisibility { get; set; } = Visibility.Visible;
        public Visibility HeaderTemperatureVisibility { get; set; } = Visibility.Collapsed;

        public byte HueBrightness
        {
            get { return HueGroup.Action.Brightness; }
            set
            {
                HueGroup.Action.Brightness = value;
                SendCommand();
            }
        }
        public bool HueIsOn
        {
            get { return HueGroup.Action.On; }
            set
            {
                HueGroup.Action.On = value;
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
                var appliance = Appliances.FirstOrDefault() as Light;
                if (appliance != null)
                {
                    var client = new HueClient(appliance.HueUser);
                    await client.SendCommandAsync(HueGroup);
                }
            }
        }
    }
}

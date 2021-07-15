using Q42.HueApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class HueUser : UserBase
    {
        public Bridge Bridge;

        public HueUser(Bridge bridge)
        {
            base.UserType = UserType.Hue;
            base.UserName = "Hue Bridge - " + bridge.Config.Name;
            base.Id = bridge.Config.BridgeId;
            base.ProfilePictureUrl = "/Assets/Icons/phillips_hue_logo.png";
            Bridge = bridge;
        }

        public HueUser() { }
    }
}

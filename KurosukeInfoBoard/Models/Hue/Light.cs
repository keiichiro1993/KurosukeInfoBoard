using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Models.Hue
{
    public class Light : IAppliance
    {
        public Light(Q42.HueApi.Light light, HueUser user)
        {
            HueLight = light;
            HueUser = user;
        }

        public Q42.HueApi.Light HueLight { get; set; }
        public HueUser HueUser { get; set; }

        public string ApplianceName { get { return HueLight.Name; } }

        public string ApplianceType { get { return HueLight.Type; } }

        public string IconImage
        {
            get
            {
                var path = Application.Current.RequestedTheme == ApplicationTheme.Light ? "ms-appx:///Assets/Icons/IRControls_black/" : "ms-appx:///Assets/Icons/IRControls_white/";
                return path + "Light_new.svg";
            }
        }
    }
}

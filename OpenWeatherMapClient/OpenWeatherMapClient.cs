using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace OpenWeatherMap
{
    public class OpenWeatherMapClient
    {
        private string key;
        public OpenWeatherMapClient()
        {
            var resource = ResourceLoader.GetForViewIndependentUse("Keys");
            key = resource.GetString("OpenWeatherMapKey");
        }


    }
}

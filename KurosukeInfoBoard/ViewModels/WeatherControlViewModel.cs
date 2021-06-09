using DebugHelper;
using KurosukeInfoBoard.Utils;
using OpenWeatherMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace KurosukeInfoBoard.ViewModels
{
    public class WeatherControlViewModel : ViewModelBase
    {
        private WeatherResponse _Weather;
        public WeatherResponse Weather
        {
            get { return _Weather; }
            set
            {
                _Weather = value;
                RaisePropertyChanged();
            }
        }


        // to auto-refresh weather
        private DateTime prevDate = DateTime.Now;
        private bool displayed = true;
        public async Task Init()
        {
            await LoadWeather();

            var span = new TimeSpan(0, 30, 0);

            while (displayed)
            {
                await Task.Delay(10000);
                if (DateTime.Now - prevDate > span)
                {
                    prevDate = DateTime.Now;

                    await LoadWeather();
                }
            }
        }

        public void StopRefreshing()
        {
            displayed = false;
        }

        private async Task LoadWeather()
        {
            IsLoading = true;

            try
            {
                var client = new OpenWeatherMap.OpenWeatherMapClient();
                var cityId = SettingsHelper.Settings.CityId.GetValue<double>();
                if (cityId == default(double))
                {
                    cityId = 2113015;//set Chiba-Japan as default value
                }
                Weather = await client.GetWeatherAsync(cityId);
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while loading weather.", ex);
                await new MessageDialog(ex.Message, "Error occured while loading weather.").ShowAsync();
            }

            IsLoading = false;
        }
    }
}

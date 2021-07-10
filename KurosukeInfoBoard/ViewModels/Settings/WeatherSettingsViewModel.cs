using KurosukeInfoBoard.Utils;
using OpenWeatherMap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class WeatherSettingsViewModel : Common.ViewModels.ViewModelBase
    {
        private List<City> _Cities;
        public List<City> Cities
        {
            get { return _Cities; }
            set
            {
                _Cities = value;
                RaisePropertyChanged();
            }
        }

        private City _SelectedCity;
        public City SelectedCity
        {
            get { return _SelectedCity; }
            set
            {
                if (_SelectedCity != value)
                {
                    _SelectedCity = value;
                    RaisePropertyChanged();

                    if (!IsLoading)
                    {
                        SettingsHelper.Settings.CityId.SetValue(value.Id);
                    }
                }
            }
        }

        private List<Country> _Countries;
        public List<Country> Countries
        {
            get { return _Countries; }
            set
            {
                _Countries = value;
                RaisePropertyChanged();

            }
        }

        private Country _SelectedCountry;
        public Country SelectedCountry
        {
            get { return _SelectedCountry; }
            set
            {
                if (_SelectedCountry != value)
                {
                    _SelectedCountry = value;
                    RaisePropertyChanged();
                    if (!IsLoading)
                    {
                        LoadCity();
                    }
                }
            }
        }



        public async Task Init()
        {
            IsLoading = true;
            Countries = await OpenWeatherMap.Locations.GetCountries();
            var cityId = SettingsHelper.Settings.CityId.GetValue<double?>();
            if (cityId != null)
            {
                var tmpcity = await OpenWeatherMap.Locations.GetCityById((double)cityId);
                SelectedCountry = (from country in Countries
                                   where country.Alpha2 == tmpcity.Country
                                   select country).FirstOrDefault();
                await LoadCityAsync();
                SelectedCity = (from city in Cities
                                where city.Id == tmpcity.Id
                                select city).FirstOrDefault();
            }
            IsLoading = false;
        }

        public async Task LoadCityAsync()
        {
            if (SelectedCountry != null)
            {
                Cities = await OpenWeatherMap.Locations.GetCities(SelectedCountry);
            }
        }

        public async void LoadCity()
        {
            if (SelectedCountry != null)
            {
                Cities = await OpenWeatherMap.Locations.GetCities(SelectedCountry);
            }
        }
    }
}

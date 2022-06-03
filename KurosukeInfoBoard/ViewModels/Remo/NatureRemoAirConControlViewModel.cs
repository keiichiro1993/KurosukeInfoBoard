using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DebugHelper;
using Newtonsoft.Json;

namespace KurosukeInfoBoard.ViewModels
{
    public class NatureRemoAirConControlViewModel : Common.ViewModels.ViewModelBase
    {
        private Appliance _Appliance;
        public Appliance Appliance
        {
            get { return _Appliance; }
            set
            {
                _Appliance = value;
                RaisePropertyChanged();
            }
        }

        private string _CurrentMode;
        public string CurrentMode
        {
            get { return _CurrentMode; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _CurrentMode = value;
                    var propertyInfo = Appliance.aircon.range.modes.GetType().GetProperty(CurrentMode);
                    if (propertyInfo != null)
                    {
                        var mode = propertyInfo.GetValue(Appliance.aircon.range.modes) as Mode;
                        if (mode != null)
                        {
                            InitTargetTempSlider(mode);
                            RaisePropertyChanged();
                            ChangeSettings();
                        }
                    }
                }
            }
        }

        private string _CurrentDirection;
        public string CurrentDirection
        {
            get { return _CurrentDirection; }
            set
            {
                _CurrentDirection = value;
                RaisePropertyChanged();
                ChangeSettings();
            }
        }

        private string _CurrentVolume;
        public string CurrentVolume
        {
            get { return _CurrentVolume; }
            set
            {
                _CurrentVolume = value;
                RaisePropertyChanged();
                ChangeSettings();
            }
        }

        private Visibility _SliderVisibility;
        public Visibility SliderVisibility
        {
            get
            {
                return (!string.IsNullOrEmpty(SliderMin) && !string.IsNullOrEmpty(SliderMax)) && double.Parse(SliderMin) < double.Parse(SliderMax) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private string _SliderMin;
        public string SliderMin
        {
            get { return _SliderMin; }
            set
            {
                _SliderMin = value;
                RaisePropertyChanged("SliderVisibility");
                RaisePropertyChanged();
            }
        }

        private string _SliderMax;
        public string SliderMax
        {
            get { return _SliderMax; }
            set
            {
                _SliderMax = value;
                RaisePropertyChanged("SliderVisibility");
                RaisePropertyChanged();
            }
        }

        private string _SliderChange;
        public string SliderChange
        {
            get { return _SliderChange; }
            set
            {
                _SliderChange = value;
                RaisePropertyChanged();
            }
        }

        private string _CurrentDegree;
        public string CurrentDegree
        {
            get { return _CurrentDegree; }
            set
            {
                if (value != _CurrentDegree)
                {
                    _CurrentDegree = value;
                    RaisePropertyChanged();
                    ChangeSettings();
                }
            }
        }


        public void Init(Appliance appliance)
        {
            this.Appliance = appliance;
            if (appliance.type == "AC")
            {
                CurrentDegree = appliance.settings.temp;
                CurrentMode = appliance.settings.mode;
                CurrentDirection = appliance.settings.dir;
                CurrentVolume = appliance.settings.vol;
            }

            isInitialized = true;
        }

        private void InitTargetTempSlider(Mode mode)
        {
            var temps = (from temp in mode.temp
                         orderby double.Parse(temp) ascending
                         select temp).ToList();

            SliderMin = temps.First();
            SliderMax = temps.Last();
            SliderChange = (double.Parse(temps[1]) - double.Parse(temps[0])).ToString();

            if (double.Parse(CurrentDegree) < double.Parse(SliderMin) || double.Parse(CurrentDegree) > double.Parse(SliderMax))
            {
                CurrentDegree = SliderMin;
            }
        }

        public async void PowerButton_Toggled(object sender, RoutedEventArgs e)
        {
            Appliance.settings.button = Appliance.settings.button.Replace("power-on", "");
            var currentState = ((ToggleSwitch)sender).IsOn ? "" : "power-off";
            if (currentState != Appliance.settings.button)
            {
                IsLoading = true;
                LoadingMessage = "Switching " + Appliance.nickname + "...";
                try
                {
                    ((ToggleSwitch)sender).IsEnabled = false;
                    var client = new NatureRemoClient(Appliance.Token);
                    var button = Appliance.settings.button == "" ? "power-off" : "";
                    var settingsString = await client.PostButton(Appliance.id, "aircon_settings", button);
                    Appliance.settings = JsonConvert.DeserializeObject<Models.NatureRemo.Settings>(settingsString);
                    ((ToggleSwitch)sender).IsEnabled = true;
                }
                catch (Exception ex)
                {
                    Debugger.WriteErrorLog("Error occured while " + LoadingMessage, ex);
                    await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
                }
                finally
                {
                    ((ToggleSwitch)sender).IsEnabled = true;
                }
                IsLoading = false;
            }
        }

        public void ModeButton_Click(object sender, RoutedEventArgs e)
        {
            var currentIndex = Appliance.aircon.range.modes.ModeList.IndexOf(CurrentMode);
            var count = Appliance.aircon.range.modes.ModeList.Count;
            if (currentIndex >= count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            CurrentMode = Appliance.aircon.range.modes.ModeList[currentIndex];
        }

        public void DirectionButton_Click(object sender, RoutedEventArgs e)
        {
            var mode = (Mode)Appliance.aircon.range.modes.GetType().GetProperty(CurrentMode).GetValue(Appliance.aircon.range.modes);
            var currentIndex = mode.dir.IndexOf(CurrentDirection);
            var count = mode.dir.Count;
            if (currentIndex >= count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            CurrentDirection = mode.dir[currentIndex];
        }

        public void VolumeButton_Click(object sender, RoutedEventArgs e)
        {
            var mode = (Mode)Appliance.aircon.range.modes.GetType().GetProperty(CurrentMode).GetValue(Appliance.aircon.range.modes);
            var currentIndex = mode.vol.IndexOf(CurrentVolume);
            var count = mode.vol.Count;
            if (currentIndex >= count - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            CurrentVolume = mode.vol[currentIndex];
        }


        private bool isInitialized = false;
        private DateTime lastModify;
        private async void ChangeSettings()
        {
            if (isInitialized)
            {
                lastModify = DateTime.Now;
                await Task.Delay(1000);
                if (DateTime.Now - lastModify > new TimeSpan(0, 0, 1))
                {
                    IsLoading = true;
                    LoadingMessage = "Sending air con settings...";

                    var settings = Appliance.settings;
                    settings.temp = CurrentDegree;
                    settings.mode = CurrentMode;
                    settings.dir = CurrentDirection;
                    settings.vol = CurrentVolume;
                    try
                    {
                        var client = new NatureRemoClient(Appliance.Token);
                        Appliance.settings = await client.PostAirConSettings(Appliance.id, settings);
                    }
                    catch (Exception ex)
                    {
                        Debugger.WriteErrorLog("Error occured while " + LoadingMessage, ex);
                        await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
                    }

                    IsLoading = false;
                }
            }
        }
    }
}

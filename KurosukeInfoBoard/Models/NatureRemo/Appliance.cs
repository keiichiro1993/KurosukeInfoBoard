using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.Models.NatureRemo
{
    public class Appliance : IAppliance
    {
        public string ApplianceName { get { return nickname; } }
        public string ApplianceType { get { return type; } }

        public TokenBase Token { get; set; }
        public string id { get; set; }
        public Device device { get; set; }
        public Model model { get; set; }
        public string type { get; set; }
        public string nickname { get; set; }
        public string image { get; set; }
        public Settings settings { get; set; }
        public Aircon aircon { get; set; }
        public List<Signal> signals { get; set; }
        public Tv tv { get; set; }

        public string IconImage
        {
            get
            {
                var path = Application.Current.RequestedTheme == ApplicationTheme.Light ? "ms-appx:///Assets/Icons/IRControls_black/" : "ms-appx:///Assets/Icons/IRControls_white/";
                if (image == "ico_light") { return path + "Light_new.svg"; }
                else if (tv != null) { return path + "TV.svg"; }
                else if (aircon != null) { return path + "AC.svg"; }
                else { return path + "Lightning.svg"; }
            }
        }
    }

    public class Model
    {
        public string id { get; set; }
        public string country { get; set; }
        public string manufacturer { get; set; }
        public string remote_name { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string series { get; set; }
    }

    public class Settings
    {
        public string temp { get; set; }
        public string temp_unit { get; set; }
        public string mode { get; set; }
        public string vol { get; set; }
        public string dir { get; set; }
        public string dirh { get; set; }
        public string button { get; set; }
        public DateTime updated_at { get; set; }
        public bool IsOn
        {
            get
            {
                return button != "power-off";
            }
        }
    }

    public class Mode
    {
        public List<string> temp { get; set; }
        public List<string> dir { get; set; }
        public List<string> dirh { get; set; }
        public List<string> vol { get; set; }
    }

    public class Modes
    {
        public Mode cool { get; set; }
        public Mode dry { get; set; }
        public Mode warm { get; set; }
        public List<string> ModeList
        {
            get
            {
                var list = (from prop in this.GetType().GetProperties()
                            select prop.Name).ToList();
                if (list.Contains("ModeList")) { list.Remove("ModeList"); };
                return list;
            }
        }
    }

    public class Range
    {
        public Modes modes { get; set; }
        public List<string> fixedButtons { get; set; }
    }

    public class Aircon
    {
        public Range range { get; set; }
        public string tempUnit { get; set; }
    }

    public class Signal
    {
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

    public class Button
    {
        public string name { get; set; }
        public string image { get; set; }
        public string label { get; set; }
    }

    public class State
    {
        public string input { get; set; }
    }

    public class Tv
    {
        public List<Button> buttons { get; set; }
        public State state { get; set; }
    }
}

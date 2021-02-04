using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace KurosukeInfoBoard.Utils.UIHelper
{
    public class ColorConverter
    {
        public static SolidColorBrush HexToBrush(string hex)
        {
            if (hex.Length < 9) { hex = hex.Replace("#", "#FF"); }
            var a = (byte)System.Convert.ToUInt32(hex.Substring(1, 2), 16);
            var r = (byte)System.Convert.ToUInt32(hex.Substring(3, 2), 16);
            var g = (byte)System.Convert.ToUInt32(hex.Substring(5, 2), 16);
            var b = (byte)System.Convert.ToUInt32(hex.Substring(7, 2), 16);
            Color color = Color.FromArgb(255, r, g, b);
            return new SolidColorBrush(color);
        }
    }
}

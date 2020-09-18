using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class AuthDialogViewModel : ViewModelBase
    {
        private bool _IsButtonAvailable = true;
        public bool IsButtonAvailable
        {
            get { return _IsButtonAvailable; }
            set
            {
                _IsButtonAvailable = value;
                RaisePropertyChanged();
            }
        }
    }
}

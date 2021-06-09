using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels.Settings
{
    public class AccountSettingsViewModel : ViewModelBase
    {
        private ObservableCollection<UserBase> _Users;
        public ObservableCollection<UserBase> Users
        {
            get { return _Users; }
            set
            {
                _Users = value;
                RaisePropertyChanged();
            }
        }

        public void Init()
        {
            Users = AppGlobalVariables.Users;
        }
    }
}

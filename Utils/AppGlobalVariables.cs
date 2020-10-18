using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Google;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class AppGlobalVariables
    {
        public static Uri GoogleAuthResultUri;
        public static ObservableCollection<UserBase> Users;
        public static Colors Colors;
    }
}

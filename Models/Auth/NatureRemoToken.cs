using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class NatureRemoToken : TokenBase
    {
        public NatureRemoToken(string token)
        {
            AccessToken = token;
            UserType = UserType.NatureRemo;
        }
    }
}

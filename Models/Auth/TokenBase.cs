﻿using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Models.Auth
{
    public class TokenBase
    {
        public UserType UserType { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }

        public string Id { get; set; }

        public bool IsTokenExpired()
        {
            return TokenExpiration == null || TokenExpiration - DateTime.Now < new TimeSpan(0, 0, 10);
        }

        public virtual async Task<TokenBase> AcquireNewToken()
        {
            switch (UserType)
            {
                case UserType.Google:
                    var newToken = await GoogleAuthClient.AcquireNewTokenWithRefreshToken(RefreshToken);
                    AccessToken = newToken.AccessToken;
                    TokenExpiration = TokenExpiration;
                    newToken.RefreshToken = RefreshToken;
                    return newToken;
                case UserType.Microsoft:
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}

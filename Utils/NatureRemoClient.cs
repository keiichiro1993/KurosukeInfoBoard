using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.NatureRemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KurosukeInfoBoard.Utils
{
    public class NatureRemoClient : HTTPClientBase
    {
        private string userInfoEndpoint = "https://api.nature.global/1/users/me";
        private string endpoint = "https://api.nature.global";

        public NatureRemoClient(TokenBase token)
        {
            this.token = token;
        }

        public async Task<NatureRemoUser> GetUserDataAsync()
        {
            var jsonString = await GetAsync(userInfoEndpoint);
            var userData = JsonSerializer.Deserialize<NatureRemoUser>(jsonString);
            userData.UserType = UserType.NatureRemo;
            userData.Token = token;
            userData.ProfilePictureUrl = "/Assets/Icons/nature_remo_logo.png";
            return userData;
        }

        public async Task<List<Appliance>> GetAppliancesAsync()
        {
            return await GetAsyncWithType<List<Appliance>>(endpoint + "/1/appliances");
        }

        public async Task<List<Device>> GetDevicesAsync()
        {
            return await GetAsyncWithType<List<Device>>(endpoint + "/1/devices");
        }

        private async Task<T> GetAsyncWithType<T>(string url)
        {
            var jsonString = await GetAsync(url);
            return JsonSerializer.Deserialize<T>(jsonString);
        }

    }
}

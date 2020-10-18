﻿using KurosukeInfoBoard.Models.NatureRemo;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace KurosukeInfoBoard.ViewModels
{
    public class NatureRemoTVControlViewModel : ViewModelBase
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

        public void Init(Appliance appliance)
        {
            this.Appliance = appliance;
        }


        public async void PowerButton_Click(object sender, RoutedEventArgs e)
        {
            IsLoading = true;
            LoadingMessage = "Sending tv signal...";

            try
            {
                var client = new NatureRemoClient(Appliance.Token);
                await client.PostTvButton(Appliance.id, "power");
            }
            catch (Exception ex)
            {
                DebugHelper.WriteErrorLog("Error occured while " + LoadingMessage, ex);
                await new MessageDialog(ex.Message, "Error occured while " + LoadingMessage).ShowAsync();
            }

            IsLoading = false;
        }
    }
}

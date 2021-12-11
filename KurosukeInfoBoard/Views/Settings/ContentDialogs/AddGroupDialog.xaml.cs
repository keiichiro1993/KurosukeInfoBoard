using Common.ViewModels;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using KurosukeInfoBoard.ViewModels.Settings.ContentDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// コンテンツ ダイアログの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace KurosukeInfoBoard.Views.ContentDialogs
{
    public sealed partial class AddGroupDialog : ContentDialog
    {
        public AddGroupDialogViewModel viewModel;
        public AddGroupDialog(ObservableCollection<IDevice> remoDevices, ObservableCollection<IDevice> hueDevices, ObservableCollection<CombinedControlEntity> combinedControls)
        {
            this.InitializeComponent();
            viewModel = new AddGroupDialogViewModel(remoDevices, hueDevices, combinedControls);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            canClose = false;
            viewModel.IsPrimaryButtonEnabled = false;

            try
            {
                await viewModel.CreateGroup();
                this.Hide();
            }
            catch (Exception ex)
            {
                DebugHelper.Debugger.WriteErrorLog("Error occurred while saving new group.", ex);
                await new MessageDialog("Error occurred while saving new group: " + ex.Message).ShowAsync();
            }

            canClose = true;
        }

        private bool canClose = true;
        public void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (!canClose) { args.Cancel = true; }
        }
    }
}

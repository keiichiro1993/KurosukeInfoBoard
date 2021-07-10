using DebugHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Memo
{
    public sealed partial class MemoControl : UserControl
    {
        public MemoControl()
        {
            this.InitializeComponent();
            this.Loaded += MemoControl_Loaded;
            this.Unloaded += MemoControl_Unloaded;
        }

        private string fileName;

        #region Dependency Properties

        /// <summary>
        /// FileName should be formatted as .gif
        /// </summary>
        public string FileName
        {
            get => (string)GetValue(FileNameProperty);
            set => SetValue(FileNameProperty, value);
        }

        public static readonly DependencyProperty FileNameProperty =
          DependencyProperty.Register(nameof(FileName), typeof(string),
            typeof(MemoControl), new PropertyMetadata(null, new PropertyChangedCallback(OnFileNameChanged)));

        #endregion

        #region Event Handlers

        private static async void OnFileNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MemoControl cc = d as MemoControl;
            cc.fileName = (string)e.NewValue;
            await cc.LoadCanvas();
        }

        private void MemoControl_Unloaded(object sender, RoutedEventArgs e)
        {
            memoCanvas.InkPresenter.StrokesCollected -= InkPresenter_StrokesCollected;
            memoCanvas.InkPresenter.StrokesErased -= InkPresenter_StrokesErased;

            this.Loaded -= MemoControl_Loaded;
            this.Unloaded -= MemoControl_Unloaded;
        }

        private void MemoControl_Loaded(object sender, RoutedEventArgs e)
        {
            memoCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
            memoCanvas.InkPresenter.StrokesErased += InkPresenter_StrokesErased;
        }

        private async void InkPresenter_StrokesErased(InkPresenter sender, InkStrokesErasedEventArgs args)
        {
            await SaveInkAfter2Secs();
        }

        private async void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            await SaveInkAfter2Secs();
        }

        #endregion

        #region Internal Functions

        private async Task LoadCanvas()
        {
            StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                if (await localFolder.TryGetItemAsync(fileName) != null)
                {
                    var file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.OpenIfExists);

                    using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        using (var inputStream = stream.GetInputStreamAt(0))
                        {
                            await memoCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
                        }
                    }
                }
                else
                {
                    memoCanvas.InkPresenter.StrokeContainer.Clear();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while loading memo canvas.", ex);
                await new MessageDialog(ex.Message, "Error occured while loading memo canvas.").ShowAsync();
            }
        }

        private DateTime lastModifiedTime = DateTime.Now;
        private async Task SaveInkAfter2Secs()
        {
            lastModifiedTime = DateTime.Now;
            await Task.Delay(2000);
            if (DateTime.Now - lastModifiedTime > new TimeSpan(0, 0, 2))
            {
                //save only if more than 2 secs from last modification
                await SaveInk();
            }
        }

        private async Task SaveInk()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.ReplaceExisting);
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
                    {
                        await memoCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                }

                // Finalize write so other apps can update file.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Debugger.WriteDebugLog("Memo ink saved.");
                }
                else
                {
                    throw new Exception("Memo ink couldn't be saved. Status=" + status.ToString() + ".");
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while saving memo canvas.", ex);
                await new MessageDialog(ex.Message, "Error occured while saving memo canvas.").ShowAsync();
            }
        }

        private void EnableTouchWritingToggle_Click(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                memoCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                memoCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }
        }

        #endregion
    }
}

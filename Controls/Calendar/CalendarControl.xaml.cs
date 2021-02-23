using KurosukeInfoBoard.Models;
using KurosukeInfoBoard.Models.Auth;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using DebugHelper;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.Calendar
{
    public sealed partial class CalendarControl : UserControl
    {
        List<CalendarItemControl> itemControls = new List<CalendarItemControl>();
        Symbol TouchWritingIcon = (Symbol)0xED5F;
        public CalendarControl()
        {
            this.InitializeComponent();
            this.Loaded += CalendarControl_Loaded;
        }

        private void CalendarControl_Loaded(object sender, RoutedEventArgs e)
        {
            FindChildren<CalendarItemControl>(itemControls, rootGrid);

            if (Application.Current.RequestedTheme == ApplicationTheme.Dark)
            {
                InkDrawingAttributes drawingAttributes = calendarCanvas.InkPresenter.CopyDefaultDrawingAttributes();
                drawingAttributes.Color = Windows.UI.Colors.White;
                calendarCanvas.InkPresenter.UpdateDefaultDrawingAttributes(drawingAttributes);
            }

            calendarCanvas.InkPresenter.StrokesCollected += InkPresenter_StrokesCollected;
        }

        private DateTime lastModifiedTime = DateTime.Now;
        private async void InkPresenter_StrokesCollected(InkPresenter sender, InkStrokesCollectedEventArgs args)
        {
            lastModifiedTime = DateTime.Now;
            await Task.Delay(2000);
            if (DateTime.Now - lastModifiedTime > new TimeSpan(0, 0, 2))
            {
                //save only if more than 2 secs from last modification

                await SaveCalendarInk();
            }
        }

        private async Task SaveCalendarInk()
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                var file = await localFolder.CreateFileAsync(_CalendarMonth.Month.ToString("yyyy-MM") + ".png", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
                    {
                        await calendarCanvas.InkPresenter.StrokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                }

                // Finalize write so other apps can update file.
                Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Debugger.WriteDebugLog("Calendar ink saved.");
                }
                else
                {
                    throw new Exception("Calendar ink couldn't be saved. Status=" + status.ToString() + ".");
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while saving calendar canvas.", ex);
                await new MessageDialog(ex.Message, "Error occured while saving calendar canvas.").ShowAsync();
            }
        }

        private CalendarMonth _CalendarMonth;
        public CalendarMonth CalendarMonth
        {
            get => (CalendarMonth)GetValue(CalendarDayProperty);
            set => SetValue(CalendarDayProperty, value);
        }

        public static readonly DependencyProperty CalendarDayProperty =
          DependencyProperty.Register(nameof(CalendarMonth), typeof(CalendarMonth),
            typeof(CalendarControl), new PropertyMetadata(null, new PropertyChangedCallback(OnCalendarMonthChanged)));

        private async static void OnCalendarMonthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CalendarControl cc = d as CalendarControl;
            var month = (CalendarMonth)e.NewValue;

            if (cc._CalendarMonth != null)
            {
                await cc.SaveCalendarInk();
            }

            cc._CalendarMonth = month;

            for (var i = 0; i < month.CalendarDays.Count; i++)
            {
                cc.itemControls[i].CalendarDay = month.CalendarDays[i];
            }

            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                if (await localFolder.TryGetItemAsync(cc.CalendarMonth.Month.ToString("yyyy-MM") + ".png") != null)
                {
                    var file = await localFolder.CreateFileAsync(cc.CalendarMonth.Month.ToString("yyyy-MM") + ".png", Windows.Storage.CreationCollisionOption.OpenIfExists);

                    using (IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                    {
                        using (var inputStream = stream.GetInputStreamAt(0))
                        {
                            await cc.calendarCanvas.InkPresenter.StrokeContainer.LoadAsync(inputStream);
                        }
                    }
                }
                else
                {
                    cc.calendarCanvas.InkPresenter.StrokeContainer.Clear();
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occured while loading calendar canvas.", ex);
                await new MessageDialog(ex.Message, "Error occured while loading calendar canvas.").ShowAsync();
            }
        }

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }

        public ObservableCollection<UserBase> Users { get { return AppGlobalVariables.Users; } }

        private void EnableTouchWritingToggle_Click(object sender, RoutedEventArgs e)
        {
            if (toggleButton.IsChecked == true)
            {
                calendarCanvas.InkPresenter.InputDeviceTypes |= CoreInputDeviceTypes.Touch;
            }
            else
            {
                calendarCanvas.InkPresenter.InputDeviceTypes &= ~CoreInputDeviceTypes.Touch;
            }
        }

        private void DisableCanvasToggle_Click(object sender, RoutedEventArgs e)
        {
            if (disableCanvasToggleButton.IsChecked == true)
            {
                calendarCanvas.Visibility = Visibility.Collapsed;
            }
            else
            {
                calendarCanvas.Visibility = Visibility.Visible;
            }
        }
    }
}

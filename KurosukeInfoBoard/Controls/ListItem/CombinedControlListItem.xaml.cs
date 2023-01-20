using Common.Models.EventArgs;
using Common.ViewModels;
using DebugHelper;
using KurosukeInfoBoard.Controls.Hue;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
using KurosukeInfoBoard.Utils.DBHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace KurosukeInfoBoard.Controls.ListItem
{
    public sealed partial class CombinedControlListItem : UserControl
    {
        public CombinedControlListItemViewModel ViewModel { get; set; } = new CombinedControlListItemViewModel();
        public CombinedControlListItem()
        {
            this.InitializeComponent();
        }

        public CombinedControl CombinedControl
        {
            get => (CombinedControl)GetValue(CombinedControlProperty);
            set => SetValue(CombinedControlProperty, value);
        }

        public static readonly DependencyProperty CombinedControlProperty =
          DependencyProperty.Register(nameof(CombinedControl), typeof(CombinedControl),
            typeof(CombinedControlListItem), new PropertyMetadata(null, new PropertyChangedCallback(OnCombinedControlChanged)));

        private static void OnCombinedControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cc = d as CombinedControlListItem;
            var combinedControl = (CombinedControl)e.NewValue;

            cc.ViewModel.Init(combinedControl);
        }

        public delegate void CombinedControlDeleteButtonClickedEventHandler(object sender, ItemDeleteButtonClickedEventArgs<CombinedControl> args);
        public event CombinedControlDeleteButtonClickedEventHandler DeleteButtonClicked;

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            button.IsEnabled = false;
            deleteItem();
            button.IsEnabled = true;
        }

        private void FlyoutDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            deleteItem();
        }

        private void deleteItem()
        {
            if (this.DeleteButtonClicked != null)
            {
                this.DeleteButtonClicked(this, new ItemDeleteButtonClickedEventArgs<CombinedControl>(CombinedControl));
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.EditUIVisibility = Visibility.Visible;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveChanges();
            ViewModel.EditUIVisibility = Visibility.Collapsed;
        }
    }

    public class CombinedControlListItemViewModel : ViewModelBase
    {
        /// <summary>
        /// raise in progress status and save changes
        /// </summary>
        public async void SaveChanges()
        {
            IsLoading = true;
            try
            {
                var dbHelper = new CombinedControlHelper();
                await dbHelper.Init();
                await dbHelper.AddUpdateCombinedControl(combinedControl);
            }
            catch (Exception ex)
            {
                await Debugger.ShowErrorDialog("Failed to update combined controls.", ex);
            }
            IsLoading = false;
        }

        private CombinedControl combinedControl;

        public void Init(CombinedControl combinedControl)
        {
            this.combinedControl = combinedControl;
            RaisePropertyChanged(nameof(IsSynchronized));
            RaisePropertyChanged(nameof(DeviceName));
        }

        public bool IsSynchronized
        {
            get { return combinedControl?.IsSynchronized ?? default(bool); }
            set
            {
                if (combinedControl != null && value != combinedControl.IsSynchronized)
                {
                    combinedControl.IsSynchronized = value;
                    SaveChanges();
                }
            }
        }

        public string DeviceName
        {
            get { return combinedControl?.DeviceName; }
            set
            {
                if (combinedControl != null && value != combinedControl.DeviceName)
                {
                    combinedControl.DeviceName = value;
                    RaisePropertyChanged();
                }
            }
        }

        private Visibility _EditUIVisibility = Visibility.Collapsed;
        public Visibility EditUIVisibility
        {
            get { return _EditUIVisibility; }
            set
            {
                _EditUIVisibility = value;
                RaisePropertyChanged();
                RaisePropertyChanged("UIVisibility");
            }
        }

        public Visibility UIVisibility
        {
            get { return _EditUIVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }
    }
}

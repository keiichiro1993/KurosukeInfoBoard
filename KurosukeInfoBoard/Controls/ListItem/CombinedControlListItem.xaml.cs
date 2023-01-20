using Common.Models.EventArgs;
using Common.ViewModels;
using KurosukeInfoBoard.Controls.Hue;
using KurosukeInfoBoard.Models.Common;
using KurosukeInfoBoard.Models.SQL;
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
            if (this.DeleteButtonClicked != null)
            {
                this.DeleteButtonClicked(this, new ItemDeleteButtonClickedEventArgs<CombinedControl>(CombinedControl));
            }
            button.IsEnabled = true;
        }
    }

    public class CombinedControlListItemViewModel : ViewModelBase
    {
        /// <summary>
        /// raise in progress status and save changes
        /// </summary>
        private async void propertyChanged() { }

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
                if (value != combinedControl.IsSynchronized)
                {
                    combinedControl.IsSynchronized = value;
                    propertyChanged();
                }
            }
        }

        public string DeviceName
        {
            get { return combinedControl?.DeviceName; }
            set
            {
                if (value != combinedControl.DeviceName)
                {
                    combinedControl.DeviceName = value;
                    propertyChanged();
                }
            }
        }
    }
}

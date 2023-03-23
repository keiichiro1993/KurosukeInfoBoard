using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;

namespace YoutubePlayer.ViewModels
{
    public class YoutubePlayerControlViewModel : Common.ViewModels.ViewModelBase
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged();
            }
        }

        private string _PlaylistName;
        public string PlaylistName
        {
            get { return _PlaylistName; }
            set
            {
                _PlaylistName = value;
                RaisePropertyChanged();
            }
        }

        private string _ChannelName;
        public string ChannelName
        {
            get { return _ChannelName; }
            set
            {
                _ChannelName = value;
                RaisePropertyChanged();
            }
        }

        private bool _IsLoading = false;
        public new bool IsLoading
        {
            get { return _IsLoading; }
            set
            {
                _IsLoading = value;
                RaisePropertyChanged();
                RaisePropertyChanged("LoadingVisibility");
            }
        }

        public Visibility LoadingVisibility
        {
            get { return IsLoading ? Visibility.Visible : Visibility.Collapsed; }
        }
    }
}

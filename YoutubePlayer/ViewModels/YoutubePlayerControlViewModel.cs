using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;

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

        private MediaSource _MediaSource;
        public MediaSource MediaSource
        {
            get { return _MediaSource; }
            set
            {
                _MediaSource = value;
                RaisePropertyChanged();
            }
        }
    }
}

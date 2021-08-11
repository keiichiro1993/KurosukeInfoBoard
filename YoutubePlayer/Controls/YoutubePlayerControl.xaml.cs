using DebugHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using YoutubePlayer.Utils;
using YoutubePlayer.ViewModels;

// ユーザー コントロールの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234236 を参照してください

namespace YoutubePlayer.Controls
{
    public sealed partial class YoutubePlayerControl : UserControl
    {
        public YoutubePlayerControlViewModel viewModel = new YoutubePlayerControlViewModel();
        public YoutubePlayerControl()
        {
            this.InitializeComponent();
            this.Loaded += YoutubePlayerControl_Loaded;
            this.Unloaded += YoutubePlayerControl_Unloaded;
        }

        private void YoutubePlayerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= YoutubePlayerControl_Loaded;
            this.Unloaded -= YoutubePlayerControl_Unloaded;
        }

        //TODO: live change
        public string YouTubePlaylistId
        {
            get => (string)GetValue(YouTubePlaylistIdProperty);
            set => SetValue(YouTubePlaylistIdProperty, value);
        }

        public static readonly DependencyProperty YouTubePlaylistIdProperty =
          DependencyProperty.Register(nameof(YouTubePlaylistId), typeof(string),
            typeof(YoutubePlayerControl), new PropertyMetadata(null, new PropertyChangedCallback(OnPlaylistIdChanged)));

        private static void OnPlaylistIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        // TODO: error handling
        private void YoutubePlayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var playlistId = "PLdhB2hC90YEuobrLs15TS2LX__iQnFdc9";
            //var playlistId = "PLYV8__7x__vtPKE-iNz3Xt7HATRSWKv3C";

            InitPlayer(YouTubePlaylistId);
        }

        private async void InitPlayer(string playlistId)
        {
            var client = new KYoutubeClient();

            try
            {
                viewModel.PlaylistName = (await client.Playlists.GetAsync(playlistId)).Title;

                while (true)
                {
                    await foreach (var batch in client.Playlists.GetVideoBatchesAsync(playlistId))
                    {
                        foreach (var video in batch.Items)
                        {
                            await PlayVideo(video, client);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occurred in YouTube Player controlk.", ex);
                await new MessageDialog("Please make sure you specified the correct playlist ID. After changing playlist ID, please restart this app. Error=" + ex.Message, "Error occurred in YouTube Player").ShowAsync();
            }
        }

        private async Task PlayVideo(YoutubeExplode.Playlists.PlaylistVideo video, KYoutubeClient client, int retry = 2)
        {
            try
            {
                viewModel.Title = video.Title;
                viewModel.ChannelName = video.Author.Title;
                viewModel.MediaSource = await client.GetHighestQualityVideoAsMediaSource(video.Id);

                Debugger.WriteDebugLog("1. " + player.MediaPlayer.PlaybackSession.PlaybackState);
                while (!(player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Paused || player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None))
                {
                    await Task.Delay(1000);
                    Debugger.WriteDebugLog("2. " + player.MediaPlayer.PlaybackSession.PlaybackState);
                }
                Debugger.WriteDebugLog("3. " + player.MediaPlayer.PlaybackSession.PlaybackState);
            }
            catch (Exception ex)
            {
                Debugger.WriteErrorLog("Error occurred with video id=[" + video.Id + "] title=[" + video.Title + "]. remaining retry=" + retry + ".", ex);
                if (retry > 0)
                {
                    retry--;
                    await PlayVideo(video, client, retry);
                }
            }
        }
    }
}

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

        // TODO: error handling
        private async void YoutubePlayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            //await PlayVideo("FJ-empM4bxE");
            //PL81YhiUKD-aSI9NrzYAAfH-usO7by0bzJ

            //var playlistId = "PLdhB2hC90YEuobrLs15TS2LX__iQnFdc9";
            var playlistId = "PLYV8__7x__vtPKE-iNz3Xt7HATRSWKv3C";

            var client = new KYoutubeClient();

            viewModel.PlaylistName = (await client.Playlists.GetAsync(playlistId)).Title;

            await foreach (var batch in client.Playlists.GetVideoBatchesAsync(playlistId))
            {
                foreach (var video in batch.Items)
                {
                    viewModel.Title = video.Title;
                    viewModel.ChannelName = video.Author.Title;
                    await PlayVideo(video.Id);

                    Debugger.WriteDebugLog("1. " + player.MediaPlayer.PlaybackSession.PlaybackState);
                    while (!(player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Paused || player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.None))
                    {
                        await Task.Delay(1000);
                        Debugger.WriteDebugLog("2. " + player.MediaPlayer.PlaybackSession.PlaybackState);
                    }
                    Debugger.WriteDebugLog("3. " + player.MediaPlayer.PlaybackSession.PlaybackState);
                }
            }

        }

        private async Task PlayVideo(string id)
        {
            var client = new KYoutubeClient();
            viewModel.MediaSource = await client.GetHighestQualityVideoAsMediaSource(id);
        }
    }
}

﻿using DebugHelper;
using FFmpegInteropX;
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

        private bool isInitialized = false;
        private bool loaded = true;
        private void YoutubePlayerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            loaded = false;
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

        public bool UseAV1Codec
        {
            get => (bool)GetValue(UseAV1CodecProperty);
            set => SetValue(UseAV1CodecProperty, value);
        }

        public static readonly DependencyProperty UseAV1CodecProperty =
          DependencyProperty.Register(nameof(UseAV1Codec), typeof(bool),
            typeof(YoutubePlayerControl), new PropertyMetadata(null, new PropertyChangedCallback(OnUseAV1CodecChanged)));

        private static void OnUseAV1CodecChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        public bool EnableAudio
        {
            get => (bool)GetValue(EnableAudioProperty);
            set => SetValue(EnableAudioProperty, value);
        }

        public static readonly DependencyProperty EnableAudioProperty =
          DependencyProperty.Register(nameof(EnableAudio), typeof(bool),
            typeof(YoutubePlayerControl), new PropertyMetadata(null, new PropertyChangedCallback(OnEnableAudioChanged)));

        private static void OnEnableAudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var youtubePlayerControl = d as YoutubePlayerControl;
            var audioEnabled = (bool)e.NewValue;
            if (audioEnabled)
            {
                youtubePlayerControl.player.ElementSoundMode = ElementSoundMode.Default;
            }
            else
            {
                youtubePlayerControl.player.ElementSoundMode = ElementSoundMode.Off;
            }
        }

        public bool EnableCaching
        {
            get => (bool)GetValue(EnableCachingProperty);
            set => SetValue(EnableCachingProperty, value);
        }

        public static readonly DependencyProperty EnableCachingProperty =
          DependencyProperty.Register(nameof(EnableCaching), typeof(bool),
            typeof(YoutubePlayerControl), new PropertyMetadata(null, new PropertyChangedCallback(OnEnableCachingChanged)));

        private static void OnEnableCachingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }


        // TODO: error handling
        private void YoutubePlayerControl_Loaded(object sender, RoutedEventArgs e)
        {
            //var playlistId = "PLdhB2hC90YEuobrLs15TS2LX__iQnFdc9";
            //var playlistId = "PLYV8__7x__vtPKE-iNz3Xt7HATRSWKv3C";
            loaded = true;
            if (!isInitialized)
            {
                isInitialized = true;
                InitPlayer(YouTubePlaylistId);
            }
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
                Debugger.WriteErrorLog("Error occurred in YouTube Player control.", ex);
                await new MessageDialog("Please make sure you specified the correct playlist ID. After changing playlist ID, please restart this app. Error=" + ex.Message, "Error occurred in YouTube Player").ShowAsync();
                isInitialized = false;
            }
        }

        private async Task PlayVideo(YoutubeExplode.Playlists.PlaylistVideo video, KYoutubeClient client, int retry = 2, TimeSpan? lastPosition = null)
        {
            try
            {
                viewModel.Title = video.Title;
                viewModel.ChannelName = video.Author.ChannelTitle;

                if (EnableCaching)
                {
                    viewModel.IsLoading = true;
                    using (var stream = await client.DownloadHighestQualityVideo(video.Id, UseAV1Codec, new Progress<double>()))
                    {
                        viewModel.IsLoading = false;
                        await InnerPlayVideo(lastPosition, stream);
                    }
                }
                else
                {
                    using (var stream = await client.GetHighestQualityVideoAsStream(video.Id, UseAV1Codec))
                    {
                        await InnerPlayVideo(lastPosition, stream);
                    }
                }
            }
            catch (Exception ex)
            {
                viewModel.IsLoading = false;
                Debugger.WriteErrorLog("Error occurred with video id=[" + video.Id + "] title=[" + video.Title + "]. remaining retry=" + retry + ".", ex);
                if (retry > 0)
                {
                    retry--;
                    await PlayVideo(video, client, retry, lastPosition);
                }
            }
        }

        private async Task InnerPlayVideo(TimeSpan? lastPosition, Stream stream)
        {
            // FFmpeg
            var config = new MediaSourceConfig();
            config.FFmpegOptions = new PropertySet {
                        { "reconnect", 1 },
                        { "reconnect_streamed", 1 },
                        { "reconnect_on_network_error", 1 },
                    };
            config.VideoDecoderMode = VideoDecoderMode.Automatic;
            config.DefaultBufferTime = TimeSpan.Zero;
            var ffmpegStream = await FFmpegMediaSource.CreateFromStreamAsync(stream.AsRandomAccessStream(), config);

            // Media Player
            using (var mediaPlayer = new MediaPlayer())
            {
                setStreamAndPlay(ffmpegStream, mediaPlayer, lastPosition);
                Debugger.WriteDebugLog("1. " + player.MediaPlayer.PlaybackSession.PlaybackState);

                TimeSpan threashold = new TimeSpan(0, 0, 0, 0, 10); //threashold to detect the video play issue

                while (player.MediaPlayer.PlaybackSession.PlaybackState != MediaPlaybackState.None)
                {
                    if (loaded)
                    {
                        await Task.Delay(5000);

                        if (lastPosition != null && player.MediaPlayer.PlaybackSession.Position - lastPosition < threashold && player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Playing)
                        {
                            Debugger.WriteDebugLog("[Auto Recovery] Detected a video playback issue with Playing state. Trying to recover...");
                            throw new InvalidOperationException("Detected a video playback issue with Playing state");
                        }

                        lastPosition = player.MediaPlayer.PlaybackSession.Position;
                        Debugger.WriteDebugLog("2. " + player.MediaPlayer.PlaybackSession.PlaybackState + " Position:" + lastPosition);

                        if (player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
                        {
                            player.MediaPlayer.Play();
                            // PlayしたはずなのにPausedのままの場合がある。（Buffering?）
                            await Task.Delay(10000);
                            if (player.MediaPlayer.PlaybackSession.PlaybackState == MediaPlaybackState.Paused)
                            {
                                Debugger.WriteDebugLog("[Auto Recovery] Detected a video playback issue with Paused state. Trying to recover...");
                                throw new InvalidOperationException("Detected a video playback issue with Paused state");
                            }
                        }
                    }
                    else
                    {
                        while (!loaded)
                        {
                            player.MediaPlayer.Pause();
                            await Task.Delay(200);
                        }
                        player.MediaPlayer.Play();
                        await Task.Delay(200);
                    }
                }
                Debugger.WriteDebugLog("3. " + player.MediaPlayer.PlaybackSession.PlaybackState);
            }
        }

        private void setStreamAndPlay(FFmpegMediaSource ffmpegStream, MediaPlayer mediaPlayer, TimeSpan? position = null)
        {
            mediaPlayer.Source = ffmpegStream.CreateMediaPlaybackItem();
            player.SetMediaPlayer(mediaPlayer);

            if (position != null) { player.MediaPlayer.PlaybackSession.Position = (TimeSpan)position; }

            player.MediaPlayer.Play();
        }
    }
}

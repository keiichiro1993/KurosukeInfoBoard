using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace YoutubePlayer.Utils
{
    public class KYoutubeClient : YoutubeClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Youtube Video ID</param>
        /// <param name="useAV1Codec">Set true to get stream with AV1 codec.</param>
        /// <returns>System.IO.Stream of highest quality video</returns>
        public async Task<Stream> GetHighestQualityVideoAsStream(string id, bool useAV1Codec)
        {
            var streamManifest = await Videos.Streams.GetManifestAsync(id);

            IVideoStreamInfo streamInfo;
            if (useAV1Codec)
            {
                streamInfo = streamManifest.GetVideoStreams().GetWithHighestVideoQuality();
            }
            else
            {
                streamInfo = streamManifest.GetVideoStreams().Where(s => !s.VideoCodec.Contains("av01")).GetWithHighestVideoQuality();
            }

            return await Videos.Streams.GetAsync(streamInfo);
        }

        /// <summary>
        /// Checks if the video is cached and downloads if not.
        /// </summary>
        /// <param name="id">Youtube Video ID</param>
        /// <param name="useAV1Codec">Set true to get stream with AV1 codec.</param>
        /// <param name="progress">Progress<double> object to get download progress in real time.</param>
        /// <returns>System.IO.Stream of video file.</returns>
        public async Task<Stream> DownloadHighestQualityVideo(string id, bool useAV1Codec, Progress<double> progress)
        {
            var localAppData = ApplicationData.Current.LocalFolder;
            var videoCacheFolder = await localAppData.CreateFolderAsync("videocache", CreationCollisionOption.OpenIfExists);

            var streamManifest = await Videos.Streams.GetManifestAsync(id);

            IVideoStreamInfo streamInfo;
            if (useAV1Codec)
            {
                streamInfo = streamManifest.GetVideoStreams().GetWithHighestVideoQuality();
            }
            else
            {
                streamInfo = streamManifest.GetVideoStreams().Where(s => !s.VideoCodec.Contains("av01")).GetWithHighestVideoQuality();
            }

            if (streamInfo == null) 
            {
                throw new InvalidDataException($"Available video stream was not found for video id '{id}'. (AV1 codec enabled: {useAV1Codec})");
            }

            var fileName = $"{id}.{streamInfo.Container}";
            var filePath = $"{videoCacheFolder.Path}/{fileName}";

            var videoFile = await videoCacheFolder.TryGetItemAsync(fileName);
            if (videoFile != null && videoFile.IsOfType(StorageItemTypes.File))
            {
                return await ((StorageFile)videoFile).OpenStreamForReadAsync();
            }
            
            await Videos.Streams.DownloadAsync(streamInfo, filePath, progress);
            videoFile = await videoCacheFolder.TryGetItemAsync(fileName);
            if (videoFile != null && videoFile.IsOfType(StorageItemTypes.File))
            {
                return await ((StorageFile)videoFile).OpenStreamForReadAsync();
            }
            else
            {
                throw new InvalidDataException($"Video file download is completed but the file is not found. (Expected file path: {filePath})");
            }
        }
    }
}

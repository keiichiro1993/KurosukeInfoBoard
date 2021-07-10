using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Core;
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
        /// <returns></returns>
        public async Task<MediaSource> GetHighestQualityVideoAsMediaSource(string id)
        {
            var streamManifest = await Videos.Streams.GetManifestAsync(id);
            var streamInfo = streamManifest.GetVideoOnlyStreams().Where(s => s.Container == Container.Mp4).GetWithHighestVideoQuality();
            var stream = await Videos.Streams.GetAsync(streamInfo);

            return MediaSource.CreateFromStream(stream.AsRandomAccessStream(), "video/mp4");
        }
    }
}

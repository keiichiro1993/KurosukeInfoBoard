using System.IO;
using System.Threading.Tasks;
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
        public async Task<Stream> GetHighestQualityVideoAsStream(string id)
        {
            var streamManifest = await Videos.Streams.GetManifestAsync(id);
            // var streamInfo = streamManifest.GetVideoOnlyStreams().Where(s => s.Container == Container.Mp4).GetWithHighestVideoQuality();
            var streamInfo = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
            return await Videos.Streams.GetAsync(streamInfo);
        }
    }
}

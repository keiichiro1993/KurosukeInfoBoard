using System.IO;
using System.Linq;
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
        public async Task<Stream> GetHighestQualityVideoAsStream(string id, bool useAV1Codec)
        {
            var streamManifest = await Videos.Streams.GetManifestAsync(id);

            IVideoStreamInfo streamInfo;
            if (useAV1Codec)
            {
                streamInfo = streamManifest.GetVideoOnlyStreams().GetWithHighestVideoQuality();
            }
            else
            {
                streamInfo = streamManifest.GetVideoOnlyStreams().Where(s => !s.VideoCodec.Contains("av01")).GetWithHighestVideoQuality();
            }

            return await Videos.Streams.GetAsync(streamInfo);
        }
    }
}

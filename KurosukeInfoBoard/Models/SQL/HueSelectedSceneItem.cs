using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KurosukeInfoBoard.Models.SQL
{
    public class HueSelectedSceneEntity
    {
        public long ID { get; set; }

        public string HueId { get; set; }

        public string RoomId { get; set; }

        public string SceneId { get; set; }

        public string LightStateJson { get; set; }
    }
}

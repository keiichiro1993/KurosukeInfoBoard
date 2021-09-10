using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.Storage;

namespace KurosukeInfoBoard.Models.SQL
{
    [Table("HueSelectedScenes")]
    public class HueSelectedSceneEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        public string HueId { get; set; }

        [Required]
        public string RoomId { get; set; }

        [Required]
        public string SceneId { get; set; }

        [Required]
        public string LightStateJson { get; set; }
    }

    public class HueSelectedSceneContext : DbContext
    {
        private static string DBFileName = "hueselectedscene.db";

        public DbSet<HueSelectedSceneEntity> HueSelectedScenes { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + ApplicationData.Current.LocalFolder.Path + "\\" + DBFileName);
        }
    }
}

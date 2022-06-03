using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KurosukeInfoBoard.Models.SQL
{
    [Table("CombinedControl")]
    public class CombinedControlEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        public string DeviceName { get; set; }

        public string RemoID { get; set; }
        public string RemoName { get; set; }

        public string HueID { get; set; }
        public string HueName { get; set; }

        public bool IsSynchronized { get; set; }
    }

    public class CombinedControlContext : DbContext
    {
        private static string DBFileName = "combinedcontrol.db";

        public DbSet<CombinedControlEntity> CombinedControls { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + ApplicationData.Current.LocalFolder.Path + "\\" + DBFileName);
        }
    }
}

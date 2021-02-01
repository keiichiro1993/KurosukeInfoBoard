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

    [Table("CalendarCache")]
    public class CalendarCacheEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        public string CalendarId { get; set; }

        [Required]
        public string CalendarName { get; set; }

        [Required]
        public bool IsEnabled { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public string UserId { get; set; }
    }

    public class CalendarCacheContext : DbContext
    {
        private static string DBFileName = "calendarcache.db";

        public DbSet<CalendarCacheEntity> CalendarCache { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + ApplicationData.Current.LocalFolder.Path + "\\" + DBFileName);
        }
    }
}

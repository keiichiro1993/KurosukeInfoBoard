using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string? RemoID { get; set; }
        public string? RemoName { get; set; }

        public string? HueID { get; set; }
        public string? HueName { get; set; }

        public bool IsSynchronized { get; set; }

        public ICollection<SynchronizedRemoItemEntity> SynchronizedRemoItems { get; set; }
    }

    [Table("SynchronizedRemoItem")]
    public class SynchronizedRemoItemEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [Required]
        public CombinedControlEntity CombinedControl { get; set; }

        [Required]
        public string ApplianceId { get; set; }
    }

    public class CombinedControlContext : DbContext
    {
        private static string DBFileName = "combinedcontrol.db";

        public DbSet<CombinedControlEntity> CombinedControls { get; internal set; }
        public DbSet<SynchronizedRemoItemEntity> SynchronizedRemoItems { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=" + DBFileName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:n relation
            modelBuilder.Entity<CombinedControlEntity>()
                .HasMany(c => c.SynchronizedRemoItems)
                .WithOne(s => s.CombinedControl);
        }
    }

}

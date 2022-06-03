﻿// <auto-generated />
using KurosukeInfoBoard.Models.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ConsoleApp1.Migrations
{
    [DbContext(typeof(CombinedControlContext))]
    partial class CombinedControlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.25");

            modelBuilder.Entity("KurosukeInfoBoard.Models.SQL.CombinedControlEntity", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HueID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HueName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSynchronized")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RemoID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RemoName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("CombinedControl");
                });

            modelBuilder.Entity("KurosukeInfoBoard.Models.SQL.SynchronizedRemoItemEntity", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ApplianceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("CombinedControlID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CombinedControlID");

                    b.ToTable("SynchronizedRemoItem");
                });

            modelBuilder.Entity("KurosukeInfoBoard.Models.SQL.SynchronizedRemoItemEntity", b =>
                {
                    b.HasOne("KurosukeInfoBoard.Models.SQL.CombinedControlEntity", "CombinedControl")
                        .WithMany("SynchronizedRemoItems")
                        .HasForeignKey("CombinedControlID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

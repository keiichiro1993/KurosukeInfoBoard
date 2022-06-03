﻿// <auto-generated />
using KurosukeInfoBoard.Models.SQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KurosukeInfoBoard.Migrations
{
    [DbContext(typeof(CombinedControlContext))]
    [Migration("20220603135304_AddSynchronizedOption")]
    partial class AddSynchronizedOption
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoMercy.Queue;

#nullable disable

namespace NoMercy.Queue.Migrations.Queue
{
    [DbContext(typeof(QueueContext))]
    partial class QueueContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("NoMercy.Queue.Models.FailedJob", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Connection")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Exception")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FailedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("Queue")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("Uuid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FailedJobs");
                });

            modelBuilder.Entity("NoMercy.Queue.Models.QueueJob", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Attempts")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AvailableAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Payload")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int?>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Queue")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReservedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("QueueJobs");
                });
#pragma warning restore 612, 618
        }
    }
}

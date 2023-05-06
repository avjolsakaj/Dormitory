﻿// <auto-generated />
using System;
using Dormitory.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dormitory.Migrations
{
    [DbContext(typeof(FindRooMateContext))]
    partial class FindRooMateContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Dormitory.DAL.Announcement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Dormitory.DAL.Application", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AnnouncementId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "AnnouncementId" }, "IX_Applications_AnnouncementId");

                    b.HasIndex(new[] { "StudentId" }, "IX_Applications_StudentId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Dormitory.DAL.Dormitory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("MaxCapacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Dormitories");
                });

            modelBuilder.Entity("Dormitory.DAL.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<int>("DormitoryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "DormitoryId" }, "IX_Rooms_DormitoryId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Dormitory.DAL.RoomStudent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("date");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("date");

                    b.Property<int>("StudentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "RoomId" }, "IX_RoomStudent_RoomId");

                    b.HasIndex(new[] { "StudentId" }, "IX_RoomStudent_StudentId");

                    b.ToTable("RoomStudent", (string)null);
                });

            modelBuilder.Entity("Dormitory.DAL.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("Dormitory.DAL.Announcement", b =>
                {
                    b.HasOne("Dormitory.DAL.Room", "Room")
                        .WithMany("Announcements")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK_Rooms_Announcement");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Dormitory.DAL.Application", b =>
                {
                    b.HasOne("Dormitory.DAL.Announcement", "Announcement")
                        .WithMany("Applications")
                        .HasForeignKey("AnnouncementId")
                        .IsRequired()
                        .HasConstraintName("FK_Applications_Announcements");

                    b.HasOne("Dormitory.DAL.Student", "Student")
                        .WithMany("Applications")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_Applications_Students");

                    b.Navigation("Announcement");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Dormitory.DAL.Room", b =>
                {
                    b.HasOne("Dormitory.DAL.Dormitory", "Dormitory")
                        .WithMany("Rooms")
                        .HasForeignKey("DormitoryId")
                        .IsRequired()
                        .HasConstraintName("FK_Rooms_Dormitories");

                    b.Navigation("Dormitory");
                });

            modelBuilder.Entity("Dormitory.DAL.RoomStudent", b =>
                {
                    b.HasOne("Dormitory.DAL.Room", "Room")
                        .WithMany("RoomStudents")
                        .HasForeignKey("RoomId")
                        .IsRequired()
                        .HasConstraintName("FK_RoomStudent_Rooms");

                    b.HasOne("Dormitory.DAL.Student", "Student")
                        .WithMany("RoomStudents")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK_RoomStudent_Students");

                    b.Navigation("Room");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Dormitory.DAL.Announcement", b =>
                {
                    b.Navigation("Applications");
                });

            modelBuilder.Entity("Dormitory.DAL.Dormitory", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("Dormitory.DAL.Room", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("RoomStudents");
                });

            modelBuilder.Entity("Dormitory.DAL.Student", b =>
                {
                    b.Navigation("Applications");

                    b.Navigation("RoomStudents");
                });
#pragma warning restore 612, 618
        }
    }
}
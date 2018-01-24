﻿// <auto-generated />
using Evant.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Evant.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Evant.DAL.EF.Tables.Address", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("AddressId");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnName("City")
                        .HasColumnType("nvarchar(20)");

                    b.Property<double>("Latitude")
                        .HasColumnName("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnName("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Town")
                        .IsRequired()
                        .HasColumnName("Town")
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnName("Icon")
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnName("Content")
                        .HasColumnType("nvarchar(140)");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("EventId")
                        .HasColumnName("EventId");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Event", b =>
                {
                    b.Property<Guid>("Id");

                    b.Property<Guid>("AddressId")
                        .HasColumnName("AddressId");

                    b.Property<Guid>("CategoryId")
                        .HasColumnName("CategoryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("Description")
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("FinishDate")
                        .HasColumnName("FinishDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsPrivate")
                        .HasColumnName("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .IsRequired()
                        .HasColumnName("Photo")
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("StartDate")
                        .HasColumnType("datetime");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.EventOperation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("EventId")
                        .HasColumnName("EventId");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventOperations");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.EventTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("EventId")
                        .HasColumnName("EventId");

                    b.Property<Guid>("TagId")
                        .HasColumnName("TagId");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("TagId");

                    b.ToTable("EventTags");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.FriendOperation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("FollowerId")
                        .HasColumnName("FollowerId");

                    b.Property<Guid>("FollowingId")
                        .HasColumnName("FollowingId");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("FollowerId");

                    b.HasIndex("FollowingId");

                    b.ToTable("FriendOperations");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Exception")
                        .HasColumnName("Exception")
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasColumnName("Ip")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Message")
                        .HasColumnName("Message")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnName("Status")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Table")
                        .IsRequired()
                        .HasColumnName("Table")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnName("Content")
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsRead")
                        .HasColumnName("IsRead")
                        .HasColumnType("bit");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasColumnName("NotificationType")
                        .HasColumnType("nvarchar(20)");

                    b.Property<Guid>("SpecialId")
                        .HasColumnName("SpecialId");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.ReportType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Level")
                        .HasColumnName("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("ReportTypes");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.SearchHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Keyword")
                        .IsRequired()
                        .HasColumnName("Keyword")
                        .HasColumnType("nvarchar(80)");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SearchHistories");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("Email")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("FacebookId")
                        .HasColumnName("FacebookId")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("FirstName")
                        .HasColumnType("nvarchar(20)");

                    b.Property<bool>("IsActive")
                        .HasColumnName("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFacebook")
                        .HasColumnName("IsFacebook")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("LastName")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("Password")
                        .HasColumnType("nvarchar(160)");

                    b.Property<string>("Photo")
                        .HasColumnName("Photo")
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnName("Role")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnName("Brand")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnName("DeviceId")
                        .HasColumnType("nvarchar(80)");

                    b.Property<bool>("IsLoggedin")
                        .HasColumnName("IsLoggedin")
                        .HasColumnType("bit");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnName("Model")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("OS")
                        .IsRequired()
                        .HasColumnName("OS")
                        .HasColumnType("nvarchar(40)");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId")
                        .HasColumnName("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("ReportTypeId")
                        .HasColumnName("ReportTypeId");

                    b.Property<Guid>("ReportedUserId")
                        .HasColumnName("ReportedUserId");

                    b.Property<Guid>("ReporterUserId")
                        .HasColumnName("ReporterUserId");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("ReportTypeId");

                    b.HasIndex("ReportedUserId");

                    b.HasIndex("ReporterUserId");

                    b.ToTable("UserReports");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Comment", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.Event", "Event")
                        .WithMany("EventComments")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("EventComments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Event", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.Category", "Category")
                        .WithMany("Events")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Evant.DAL.EF.Tables.Address", "EventAddress")
                        .WithOne("Event")
                        .HasForeignKey("Evant.DAL.EF.Tables.Event", "Id")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("Events")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.EventOperation", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.Event", "Event")
                        .WithMany("EventOperations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("EventOperations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.EventTag", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.Event", "Event")
                        .WithMany("EventTags")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.Tag", "Tag")
                        .WithMany("EventTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.FriendOperation", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "FollowerUser")
                        .WithMany("Followers")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "FollowingUser")
                        .WithMany("Followings")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Notification", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.SearchHistory", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("UserSearchHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserDevice", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserReport", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.ReportType", "ReportType")
                        .WithMany("UserReports")
                        .HasForeignKey("ReportTypeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "ReportedUser")
                        .WithMany("ReportedUsers")
                        .HasForeignKey("ReportedUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "ReporterUser")
                        .WithMany("ReporterUsers")
                        .HasForeignKey("ReporterUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}

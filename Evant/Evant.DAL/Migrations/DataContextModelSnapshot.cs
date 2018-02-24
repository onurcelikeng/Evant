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

            modelBuilder.Entity("Evant.DAL.EF.Tables.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Icon")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("EventId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("FinishDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPrivate");

                    b.Property<double>("Latitude");

                    b.Property<double>("Longitude");

                    b.Property<string>("Photo");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.Property<int>("TotalComments");

                    b.Property<int>("TotalParticipants");

                    b.Property<string>("Town")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

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

                    b.Property<Guid>("EventId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId");

                    b.ToTable("EventOperations");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.FriendOperation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<Guid>("FollowerUserId");

                    b.Property<Guid>("FollowingUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.HasIndex("FollowerUserId");

                    b.HasIndex("FollowingUserId");

                    b.ToTable("FriendOperations");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.GameBoard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("OperationType")
                        .IsRequired();

                    b.Property<int>("Point");

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("GameBoards");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.Log", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Exception");

                    b.Property<string>("Ip");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Message");

                    b.Property<int>("StatusCode");

                    b.Property<string>("Table")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.SearchHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Keyword")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SearchHistories");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthdate");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FacebookId");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsBusinessAccount");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsFacebook");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Phone");

                    b.Property<string>("Photo");

                    b.Property<string>("Role")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserDevice", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Brand")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceId")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsLoggedin");

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<string>("OS")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<bool>("IsCommentNotif");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEventNewComerNotif");

                    b.Property<bool>("IsEventUpdateNotif");

                    b.Property<bool>("IsFriendshipNotif");

                    b.Property<bool>("IsTwoFactorAuthentication");

                    b.Property<string>("Language")
                        .IsRequired();

                    b.Property<string>("Theme")
                        .IsRequired();

                    b.Property<DateTime>("UpdateAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");
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

            modelBuilder.Entity("Evant.DAL.EF.Tables.FriendOperation", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "FollowerUser")
                        .WithMany("Followings")
                        .HasForeignKey("FollowerUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Evant.DAL.EF.Tables.User", "FollowingUser")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingUserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Evant.DAL.EF.Tables.GameBoard", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithMany("GameBoard")
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

            modelBuilder.Entity("Evant.DAL.EF.Tables.UserSetting", b =>
                {
                    b.HasOne("Evant.DAL.EF.Tables.User", "User")
                        .WithOne("Setting")
                        .HasForeignKey("Evant.DAL.EF.Tables.UserSetting", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

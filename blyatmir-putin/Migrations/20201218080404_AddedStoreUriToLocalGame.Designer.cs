﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using blyatmir_putin.Core.Database;

namespace blyatmir_putin.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201218080404_AddedStoreUriToLocalGame")]
    partial class AddedStoreUriToLocalGame
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("blyatmir_putin.Core.Models.Guild", b =>
                {
                    b.Property<ulong>("GuildId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("AnnouncmentChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableGameNotifier")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableIntroMusic")
                        .HasColumnType("INTEGER");

                    b.Property<double>("FTriggerCoolDown")
                        .HasColumnType("REAL");

                    b.Property<int>("FTriggerCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GuildName")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("QuoteChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuildId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("blyatmir_putin.Core.Models.LocalGame", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BannerUri")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Developer")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Posted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PosterUri")
                        .HasColumnType("TEXT");

                    b.Property<string>("Publisher")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("StorePageUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("blyatmir_putin.Core.Models.User", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IntroSong")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
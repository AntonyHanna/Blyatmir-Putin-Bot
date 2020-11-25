﻿// <auto-generated />
using System;
using Blyatmir_Putin_Bot.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Blyatmir_Putin_Bot.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201124045446_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Blyatmir_Putin_Bot.Model.Container", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PermisssionLevel")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("Blyatmir_Putin_Bot.Model.Guild", b =>
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

            modelBuilder.Entity("Blyatmir_Putin_Bot.Model.LocalGame", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Developer")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Posted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Publisher")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Blyatmir_Putin_Bot.Model.User", b =>
                {
                    b.Property<ulong>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ContainerAccessLevel")
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

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using project_fob.Data;
using System;

namespace projectfob.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("project_fob.Models.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FobId");

                    b.Property<int?>("MeetingId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FobId");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendee");
                });

            modelBuilder.Entity("project_fob.Models.Fob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AttendeeCount");

                    b.Property<int>("FobCount");

                    b.Property<int?>("MeetingId");

                    b.Property<DateTime>("TopicStartTime");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("Fob");
                });

            modelBuilder.Entity("project_fob.Models.Host", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MeetingId");

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("Host");
                });

            modelBuilder.Entity("project_fob.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("HostPassword");

                    b.Property<string>("MeetingId")
                        .HasMaxLength(9);

                    b.Property<string>("RoomPassword");

                    b.HasKey("Id");

                    b.ToTable("Meeting");
                });

            modelBuilder.Entity("project_fob.Models.Stats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Attendeescount");

                    b.Property<int>("Fobcount");

                    b.Property<int?>("MeetingId");

                    b.Property<DateTime>("TopicStartTime");

                    b.Property<DateTime>("TopicStopTime");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("project_fob.Models.StatsClick", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ClickTime");

                    b.Property<int?>("StatsId");

                    b.HasKey("Id");

                    b.HasIndex("StatsId");

                    b.ToTable("StatsClick");
                });

            modelBuilder.Entity("project_fob.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Lastcheckin");

                    b.Property<string>("UserId")
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("project_fob.Models.Attendee", b =>
                {
                    b.HasOne("project_fob.Models.Fob")
                        .WithMany("fobbed")
                        .HasForeignKey("FobId");

                    b.HasOne("project_fob.Models.Meeting", "Meeting")
                        .WithMany("Attendee")
                        .HasForeignKey("MeetingId");

                    b.HasOne("project_fob.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("project_fob.Models.Fob", b =>
                {
                    b.HasOne("project_fob.Models.Meeting", "Meeting")
                        .WithMany()
                        .HasForeignKey("MeetingId");
                });

            modelBuilder.Entity("project_fob.Models.Host", b =>
                {
                    b.HasOne("project_fob.Models.Meeting", "Meeting")
                        .WithMany("Host")
                        .HasForeignKey("MeetingId");

                    b.HasOne("project_fob.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("project_fob.Models.Stats", b =>
                {
                    b.HasOne("project_fob.Models.Meeting")
                        .WithMany("Stats")
                        .HasForeignKey("MeetingId");
                });

            modelBuilder.Entity("project_fob.Models.StatsClick", b =>
                {
                    b.HasOne("project_fob.Models.Stats")
                        .WithMany("Clicks")
                        .HasForeignKey("StatsId");
                });
#pragma warning restore 612, 618
        }
    }
}

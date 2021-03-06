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

            modelBuilder.Entity("project_fob.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("HostPassword");

                    b.Property<string>("MeetingId")
                        .HasMaxLength(6);

                    b.Property<string>("RoomPassword");

                    b.Property<int>("TopicCounter");

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

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("project_fob.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("MeetingId");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("project_fob.Models.Stats", b =>
                {
                    b.HasOne("project_fob.Models.Meeting")
                        .WithMany("Stats")
                        .HasForeignKey("MeetingId");
                });

            modelBuilder.Entity("project_fob.Models.User", b =>
                {
                    b.HasOne("project_fob.Models.Meeting")
                        .WithMany("Fobbed")
                        .HasForeignKey("MeetingId");
                });
#pragma warning restore 612, 618
        }
    }
}

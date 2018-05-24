using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using project_fob.Controllers;
using project_fob.Data;
using project_fob.Models;
using Xunit;
using Xunit.Abstractions;

namespace unit_tests
{
    public class DatabaseFixture : IDisposable
    {
        public ApplicationDbContext DbContext { get; private set; }

        public int value { get; }


        public DatabaseFixture()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(databaseName: "testdatabase").Options;
            DbContext = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }

    public class MeetingHandlerTest : IClassFixture<DatabaseFixture>
    {

        DatabaseFixture dbFixture;

        public MeetingHandlerTest(DatabaseFixture dbFixture)
        {
            this.dbFixture = dbFixture;
        }

        [Theory]
        [InlineData("asd", "hey", "holly")]
        [InlineData("proly", "", "aa")]
        [InlineData("peep", "krub", "puddle")]
        public void CreateMeeting(string meetingId, string attendeePassword, string hostPassword)
        {
            Meeting meeting = MeetingHandler.CreateMeetingWithUniqueId(meetingId, attendeePassword, hostPassword, dbFixture.DbContext);
            bool succeeded = false;


            if (!meeting.RoomPassword.Equals(attendeePassword))
            {
                attendeePassword.Should().Be(meeting.RoomPassword);
            }
            if (!meeting.HostPassword.Equals(hostPassword))
            {
                hostPassword.Should().Be(meeting.HostPassword);
            }
            if (!meeting.MeetingId.Equals(meetingId))
            {
                meetingId.Should().Be(meeting.MeetingId.ToString());
            }
            succeeded = true;
            succeeded.Should().Be(true);
        }

        [Theory]
        [InlineData("test", "hey", "hello")]
        [InlineData("test55", "ban", "kick")]
        [InlineData("xorand", "xand", "or")]
        public void CheckDuplicateId(string meetingId, string attendeePassword, string hostPassword)
        {
            Meeting meeting = MeetingHandler.CreateMeetingWithUniqueId(meetingId, attendeePassword, hostPassword, dbFixture.DbContext);
            dbFixture.DbContext.SaveChanges();

            Meeting meeting2 = MeetingHandler.CreateMeetingWithUniqueId(meetingId, attendeePassword, hostPassword, dbFixture.DbContext);
            meeting2.MeetingId.Equals(meeting.MeetingId).Should().Be(false);
        }


    }
}

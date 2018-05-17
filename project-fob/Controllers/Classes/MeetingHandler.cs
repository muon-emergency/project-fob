using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_fob.Data;
using project_fob.Models;

namespace project_fob.Controllers
{
    public static class MeetingHandler
    {
        private static Meeting CreateMeeting(string meetingId, string attendeePassword, string hostPassword, ApplicationDbContext db)
        {
            Meeting meeting = new Meeting { MeetingId = meetingId, RoomPassword = attendeePassword, HostPassword = hostPassword, Active = true };
            db.Meeting.Add(meeting);
            return meeting;
        }

        public static Meeting CreateMeetingWithUniqueId(string meetingId, string attendeePassword, string hostPassword, ApplicationDbContext db)
        {
            Meeting meeting = CreateMeeting(meetingId, attendeePassword, hostPassword, db);
            while (db.Meeting.Any(m => m.MeetingId.Equals(meeting.MeetingId.ToString())))
            {
                meeting.MeetingId = IDGenerators.GenerateMeetingId();
            }
            return meeting;

        }

        public static void UserPressedFob(Meeting meeting, Guid id, ApplicationDbContext db)
        {
            meeting.Fobbed.Add(db.User.Find(id) ?? new User { Id = id });
        }

        public static void RestartFobbed(Meeting meeting)
        {
            meeting.Fobbed.Clear();
            meeting.TopicCounter++;
        }

        public static Meeting GetMeeting(string meetingId, ApplicationDbContext db)
        {
            return db.Meeting.Single(x => x.MeetingId == meetingId);
        }

        public static Meeting GetMeetingWithFobbed(string meetingId, ApplicationDbContext db)
        {
            return db.Meeting.Include(x => x.Fobbed).Single(x => x.MeetingId == meetingId);
        }

        public static Meeting GetMeetingWithStats(string meetingId, ApplicationDbContext db)
        {
            return db.Meeting.Include(x => x.Stats).Single(x => x.MeetingId == meetingId);
        }

        public static void FinishMeeting(Meeting meeting, ApplicationDbContext db)
        {
            meeting.Stats.Add(new Stats { Attendeescount = 0, Fobcount = meeting.Fobbed.Count });
            meeting.Active = false;
            db.SaveChanges();
        }
    }
}

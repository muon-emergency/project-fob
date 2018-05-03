using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project_fob.Data;
using project_fob.Models;

namespace project_fob.Controllers
{
    public static class MeetingWrapper
    {
        public static Meeting CreateMeeting(string meetingId, string attendeePassword, string hostPassword, ApplicationDbContext db)
        {
            Meeting meeting = new Meeting { MeetingId = meetingId, RoomPassword = attendeePassword, HostPassword = hostPassword, Active = true };
            db.Meeting.Add(meeting);
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
    }
}

using project_fob.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace project_fob.Models {
    public class Fob {
        [Key]
        public int Id { get; set; }
        public int AttendeeCount { get; set; }
        public int FobCount { get; set; }
        public DateTime TopicStartTime { get; set; }
        public virtual Meeting Meeting { get; set; }
        public virtual List<Attendee> fobbed { get; set; }


        public Fob() { }
        public Fob(Meeting meeting) { // maybe check if a meeting already has a fob ? also need to check that is active
            Meeting = meeting;
            fobbed = new List<Attendee>();
            AttendeeCount = 0;
            FobCount = 0;
            TopicStartTime = DateTime.Now;

        }
        public static Fob getFob(string meetingid, ApplicationDbContext db) {
            Fob fob =
            db.Fob.SingleOrDefault(
                f => f.Meeting.MeetingId.Equals(
                    meetingid.ToString()));
            return fob;

        }


    }
}                                             
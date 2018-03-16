using Microsoft.EntityFrameworkCore;
using project_fob.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace project_fob.Models
{
    public class Fob
    {
        [Key]
        public int Id { get; set; }

        //Technically this 2 value can be a simple int value which can be set in case something happens. However I have to change it now so I can find some of the bugs
        //which affect the counters. The set attribute is actually doing nothing (theoritically does not change the result)

            //This variable and code is mainly here because of past values are bein loaded into the project.
        public int AttendeeCount
        {
            get
            {
                if (Meeting != null)
                { return -1; }
                else { return -11; }
            }
            set { }
        }
        public int FobCount { get { return Fobbed.Count; } set { } }


        public DateTime TopicStartTime { get; set; }
        public Meeting Meeting { get; set; }
        public List<Attendee> Fobbed { get; set; } = new List<Attendee>();


        public Fob() { }
        public Fob(Meeting meeting)
        {
            // maybe check if a meeting already has a fob ? also need to check that is active
            Meeting = meeting;
            Fobbed = new List<Attendee>();
            //AttendeeCount = 0;
            //FobCount = 0;
            TopicStartTime = DateTime.Now;
        }

        public static Fob getFob(string meetingid, ApplicationDbContext db)
        {
            return db.Fob.Include(x=> x.Meeting).ThenInclude(x=>x.Attendee).ThenInclude(x=>x.User).SingleOrDefault(f => f.Meeting.MeetingId == meetingid);
        }

        public void AddFob(Attendee attendee)
        {
            if (!Fobbed.Contains(attendee))
            {
                Fobbed.Add(attendee);
            }
        }

        public void RestartFobbed()
        {
            Fobbed.Clear();
        }

        /*public int GetAttendeeCount()
        {
            return Meeting.GetAttendeeCount();
        }*/
    }
}

using Microsoft.EntityFrameworkCore;
using project_fob.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace project_fob.Models
{
    public class Topic
    {
        [Key]
        public int Id { get; set; }

        public int TopicValue { get; set; }

        public DateTime TopicStartTime { get; set; }
        public Meeting Meeting { get; set; }

        public List<User> Fobbed { get; set; } = new List<User>();
        
        public Topic() { }
        public Topic(Meeting meeting)
        {
            Meeting = meeting;
            Fobbed = new List<User>();
            TopicStartTime = DateTime.Now;
            TopicValue = 0;
        }

        public static Topic getFob(string meetingid, ApplicationDbContext db)
        {
            return db.Fob.Include(x => x.Meeting).Include(x=> x.Fobbed).SingleOrDefault(f => f.Meeting.MeetingId == meetingid);
        }

        public void AddFob(string id)
        {
            if (!Fobbed.Any(x => x.Equals(id)))
            {
                Fobbed.Add(new User(id));
            }
        }

        public void RestartFobbed()
        {
            Fobbed.Clear();
            TopicValue++;
        }
    }
}

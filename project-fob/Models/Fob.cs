using Microsoft.EntityFrameworkCore;
using project_fob.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace project_fob.Models
{
    public class Fob
    {
        [Key]
        public int Id { get; set; }

        public DateTime TopicStartTime { get; set; }
        public Meeting Meeting { get; set; }

        public List<User> Fobbed { get; set; } = new List<User>();
        
        public Fob() { }
        public Fob(Meeting meeting)
        {
            Meeting = meeting;
            Fobbed = new List<User>();
            TopicStartTime = DateTime.Now;
        }

        public static Fob getFob(string meetingid, ApplicationDbContext db)
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
        }
    }
}

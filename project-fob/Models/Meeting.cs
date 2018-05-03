using project_fob.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace project_fob.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }

        [StringLength(6)]
        public string MeetingId { get; set; }
        [Required]
        public bool Active { get; set; }
        public string RoomPassword { get; set; }
        public string HostPassword { get; set; }

        public List<Stats> Stats { get; set; }



        public int TopicCounter { get; set; }

        public List<User> Fobbed { get; set; } = new List<User>();


        public Meeting() { }

        public Meeting(string meetingid, string attendeePassword, string hostPassword)
        {
            Active = true;
            MeetingId = meetingid;
            RoomPassword = attendeePassword;
            HostPassword = hostPassword;
            Stats = new List<Stats>();
        }

        internal void UserPressedFob(Guid id, Data.ApplicationDbContext db)
        {
            Fobbed.Add(db.User.Find(id) ?? new User { Id = id });
        }

        public void RestartFobbed()
        {
            Fobbed.Clear();
            TopicCounter++;
        }
    }
}

using project_fob.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace project_fob.Models
{
    public class Meeting
    {
        [Key]
        public int Id { get; set; }

        [StringLength(9)]
        public string MeetingId { get; set; }
        [Required]
        public bool Active { get; set; }
        public string RoomPassword { get; set; }
        public string HostPassword { get; set; }

        public List<Stats> Stats { get; set; }
        public List<Host> Host { get; set; }
        public List<Attendee> Attendee { get; set; }

        public Meeting() { }

        public Meeting(string meetingid, Host host, string attendeePassword, string hostPassword)
        {
            Active = true;
            MeetingId = meetingid;
            RoomPassword = attendeePassword;
            HostPassword = hostPassword;
            Host = new List<Host>() { host };
            Stats = new List<Stats>();
            Attendee = new List<Attendee>();
        }

        public void AddAttendee(Attendee attendee)
        {
            Attendee.Add(attendee);
        }

        public int GetAttendeeCount()
        {
            return Attendee.Count;
        }
    }
}

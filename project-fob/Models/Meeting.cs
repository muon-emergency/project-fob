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
        [Index("IX_FirstAndSecond", 1, IsUnique = true)]
        [Column(TypeName = "VARCHAR")]
        [StringLength(9)]
        public string MeetingId { get; set; }
        [Index("IX_FirstAndSecond", 2, IsUnique = true)]
        [Required]
        public bool Active { get; set; }
        public string RoomPassword { get; set; }
        public string HostPassword { get; set; }
        
        //TODO List of stats
        public virtual List<Stats> Stats { get; set; }
        public virtual List<Host> Host { get; set;}
        public virtual List<Attendee> Attendee { get; set; }

        public Meeting() { }

        public Meeting(string meetingid, Host host, string attendeePassword, string hostPassword){
                Active = true;
                MeetingId = meetingid;
                RoomPassword = attendeePassword;
                HostPassword = hostPassword;
                Host = new List<Host>() { host };
                Stats = new List<Stats>();
                Attendee = new List<Attendee>();



        }   

    }
}
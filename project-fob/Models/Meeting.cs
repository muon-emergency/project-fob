using project_fob.Controllers;
using project_fob.Data;
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
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace project_fob.Models
{
    public class Stats
    {
        [Key]
        public int Id { get; set; }
        public int Attendeescount { get; set; }
        public int Fobcount { get; set; }
    }
}

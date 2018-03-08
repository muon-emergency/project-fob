using System;
using System.ComponentModel.DataAnnotations;

namespace project_fob.Models 
{
    public class Attendee 
    {
        [Key]
        public int Id { get; set; }
        public virtual Meeting Meeting { get; set; }
        public virtual User User { get; set; }

        public Attendee() 
        {
        }

        public Attendee(User user, Meeting meeting)
        {
            Meeting = meeting;
            User = user;
        }
    }
}

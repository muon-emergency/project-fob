using System;
using System.Collections.Generic;
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

        public override bool Equals(object obj)
        {
            Attendee parameter = (Attendee)obj;
            if (parameter.User.UserId == User.UserId)
            {
                if (parameter.Meeting.MeetingId == Meeting.MeetingId)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1379548420;
            hashCode = hashCode * -1521134295 + EqualityComparer<Meeting>.Default.GetHashCode(Meeting);
            hashCode = hashCode * -1521134295 + EqualityComparer<User>.Default.GetHashCode(User);
            return hashCode;
        }
    }
}

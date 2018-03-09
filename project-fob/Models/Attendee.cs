using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_fob.Models 
{
    public class Attendee 
    {
        [Key]
        public int Id { get; set; }
        public Meeting Meeting { get; set; }
        public  User User { get; set; }

        public Attendee() 
        {
        }

        public Attendee(User user, Meeting meeting)
        {
            if (user == null)
            {

            }
            Meeting = meeting;
            User = user;
        }

        public override bool Equals(object obj)
        {
            if(obj is Attendee paramater)
            {
                return Equals(paramater);
            }
            
            return false;
        }

        public bool Equals(Attendee parameter)
        {
            if (User.Equals(parameter.User))
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

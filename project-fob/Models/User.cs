using project_fob.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace project_fob.Models 
{
    public class User 
    {
        [Key]
        public int Id { get; set; }

        [StringLength(9)]
        public string UserId { get; set; }
        public virtual DateTime Lastcheckin { get; set; }

        public User() { }

        public User(string id)
        {
            UserId = id;
            Lastcheckin = DateTime.Now;
        }
        public override bool Equals(object obj)
        {
            if (obj is User paramater)
            {
                return Equals(paramater);
            }
            return false;
            //return base.Equals(obj);
        }

        private bool Equals(User parameter)
        {
            if (parameter.UserId.Equals(UserId))
            {
                    return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = -1403531409;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserId);
            hashCode = hashCode * -1521134295 + Lastcheckin.GetHashCode();
            return hashCode;
        }
    }
}

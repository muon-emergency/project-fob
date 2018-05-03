using Microsoft.EntityFrameworkCore;
using project_fob.Controllers;
using project_fob.Data;
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
        public Guid Id { get; set; }

        public User() { }

        public User(Guid id)
        {
            Id = id;
        }

        static public User GetOrCreateUser(Guid id, ApplicationDbContext db)
        {
            User user = db.User.SingleOrDefault(x=>x.Id.Equals(id));
            if (user == null)
            {
                user = new User(id);
            }
            return user;
        }

        public override bool Equals(object obj)
        {
            if (obj is User paramater)
            {
                return Equals(paramater);
            }
            if (obj is String parameter)
            {
                Guid result;
                Guid.TryParse(parameter, out result);
                return Equals(result);
            }
            return false;
        }

        //private bool Equals(User parameter)
        //{
        //    if (parameter.Equals(Id))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public override int GetHashCode()
        //{
        //    var hashCode = -1403531409;
        //    hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UserId);
        //    return hashCode;
        //}
    }
}

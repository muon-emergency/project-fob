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
    }
}

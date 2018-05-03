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
    }
}

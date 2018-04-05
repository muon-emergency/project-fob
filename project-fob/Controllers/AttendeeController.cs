using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_fob.Data;
using project_fob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_fob.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly ApplicationDbContext db;

        public AttendeeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public void Fob(string value)
        {
            HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string meetingString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.Fobbed).ThenInclude(x => x.User).Single(f => f.Meeting.MeetingId == meetingString);

            HttpContext.Session.TryGetValue("sessionid", out var session);

            string userIdString = System.Text.Encoding.ASCII.GetString(session);
            Attendee att = db.Attendee.Include(at => at.User).Include(at => at.Meeting).Single(at => at.User.UserId.Equals(userIdString) && at.Meeting.MeetingId.Equals(meetingString));

            fob.AddFob(att);
            db.SaveChanges();
        }

        public ActionResult ExitMeeting(string value)
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public string ImStillHere()
        {
            return "";
        }
    }
}

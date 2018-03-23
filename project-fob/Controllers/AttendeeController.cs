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
            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string MeetingString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.Fobbed).ThenInclude(x => x.User).Single(f => f.Meeting.MeetingId == MeetingString);

            gotvalue = HttpContext.Session.TryGetValue("sessionid", out var session);

            string UserIdString = System.Text.Encoding.ASCII.GetString(session);
            Attendee att = db.Attendee.Include(at => at.User).Include(at => at.Meeting).Single(at => at.User.UserId.Equals(UserIdString) && at.Meeting.MeetingId.Equals(MeetingString));

            fob.AddFob(att);
            db.SaveChanges();

            //return View("~/Views/Home/MeetingPageUser.cshtml");
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

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

        public ActionResult Fob(string value, string meetingString)
        {
            if (CookieHandler.HaveCookieId(out var id, new RealCookies(this)))
            {
                Meeting meeting = db.Meeting.Include(x => x.Fobbed).Single(f => f.MeetingId == meetingString);
                if (meeting.Active)
                {
                    MeetingHandler.UserPressedFob(meeting, id, db);

                    db.SaveChanges();
                    return Ok();
                }
                else
                {
                    return Content("closed");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ExitMeeting(string value)
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult ImStillHere(string value, string meetingString)
        {
            if (CookieHandler.HaveCookieId(out var userId, new RealCookies(this)))
            {
                Meeting meeting = MeetingHandler.GetMeetingWithFobbed(meetingString, db);
                int topic = meeting.TopicCounter;
                if (!meeting.Active)
                {
                    topic = -1;
                }

                return Content(topic + "");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}

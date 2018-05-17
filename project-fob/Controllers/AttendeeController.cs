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
            if (HaveCookieId(out var id))
            {
                string userId = GetCookieId();

                Meeting meeting = db.Meeting.Include(x=> x.Fobbed).Single(f => f.MeetingId == meetingString);

                MeetingWrapper.UserPressedFob(meeting, id, db);

                db.SaveChanges();
                return Ok();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private bool HaveCookieId(out Guid id)
        {
            string strignId = Request.Cookies["ID"];
            if (strignId == null || strignId.Length == 0)
            {
                return false;
            }

            return Guid.TryParse(strignId, out id);
        }

        private string GetCookieId()
        {
            //This method does not check if the cookie is correct or not. Please use HaveCookieId
            return Request.Cookies["ID"];
        }

        public ActionResult ExitMeeting(string value)
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult ImStillHere(string value, string meetingString)
        {
            if (HaveCookieId(out var userId))
            {
                Meeting meeting = db.Meeting.Include(x => x.Fobbed).Single(f => f.MeetingId == meetingString);
                int topic = meeting.TopicCounter;
                if (!meeting.Active)
                {
                    topic = -1;
                }

                return Content(topic+"");
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}

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
            if (HaveCookieId())
            {
                string userId = GetCookieId();

                Topic fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x=> x.Fobbed).Single(f => f.Meeting.MeetingId == meetingString);
                
                fob.AddFob(userId);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private bool HaveCookieId()
        {
            string id = Request.Cookies["ID"];
            if (id == null || id.Length == 0)
            {
                return false;
            }
            return true;
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
            if (HaveCookieId())
            {
                string userId = GetCookieId();

                Topic fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.Fobbed).Single(f => f.Meeting.MeetingId == meetingString);
                int topic = fob.TopicValue;
                if (!fob.Meeting.Active)
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

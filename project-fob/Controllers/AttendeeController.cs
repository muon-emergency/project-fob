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
            if (HaveCookieId())
            {
                string userId = GetCookieId();
                HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

                string meetingString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

                Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Single(f => f.Meeting.MeetingId == meetingString);
                
                fob.AddFob(userId);
                db.SaveChanges();

            }
            else
            {
                Index();
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

        public string ImStillHere()
        {
            return "";
        }
    }
}

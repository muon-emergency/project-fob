using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using project_fob.Models;
using System.Text;
using project_fob.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace project_fob.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            ViewBag.title = "Project-fob:";

            return View();
        }

        public ActionResult TestPage()
        {
            return View();
        }

        //Create Meeting
        public ActionResult MeetingPageHost(string attendeePassword, string hostPassword)
        {
            attendeePassword = attendeePassword ?? "";
            if (!CheckPasswordsAreCorrectForHosting(attendeePassword, hostPassword))
            {
                ViewBag.title = "Error: Password error";
                return View("index");
            }

            CookieHandler.CreateOrUpdateCookies(this);

            Meeting meet = MeetingHandler.CreateMeetingWithUniqueId(IDGenerators.GenerateMeetingId(), attendeePassword, hostPassword, db);

            db.Meeting.Add(meet);
            db.SaveChanges();

            ViewBag.title = "Meeting Id: ";
            ViewBag.meetingid = meet.MeetingId;

            return View();
        }

        public ActionResult MeetingPageUser(string meetingId, string password)
        {
            password = password ?? "";
            meetingId = meetingId ?? "";
            meetingId = meetingId.ToUpper();
            //Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId == meetingId);
            Meeting meet = MeetingHandler.GetMeeting(meetingId, db);

            if (meet != null && meetingId.Equals(meet.MeetingId))
            {
                //host
                if (password.Equals(meet.HostPassword))
                {
                    return JoinAsHost(meet);
                }
                else if (password == meet.RoomPassword && meet.Active)
                {
                    //join as attendee
                    return JoinAsAttendee(meetingId);
                }
                else
                {
                    //meeting pass incorrect
                    ViewBag.title = "Meeting Password Incorrect. Please Try Again";
                    return View("Index");
                }
            }
            ViewBag.title = "Meeting Does Not Exist. Please Try Again";

            return View("Index");
        }

        private ActionResult JoinAsHost(Meeting meet)
        {
            CookieHandler.ManageAndReturnCookie(this);

            if (meet.Active)
            {
                ViewBag.title = "Meeting Id: ";
                ViewBag.meetingid = meet.MeetingId;

                db.SaveChanges();
                return View("MeetingPageHost");
            }
            else
            {
                return View("~/Views/Home/StatScreen.cshtml");
            }
        }

        private ActionResult JoinAsAttendee(string meetingId)
        {
            CookieHandler.ManageAndReturnCookie(this);

            ViewBag.title = "Id: ";
            ViewBag.meetingid = meetingId;
            db.SaveChanges();
            return View();
        }

        public static bool CheckPasswordsAreCorrectForHosting(string attendeePassword, string hostPassword)
        {
            attendeePassword = attendeePassword ?? "";
            if (hostPassword == null || hostPassword.Length == 0)
            {
                return false;
            }
            if (hostPassword.Equals(attendeePassword))
            {
                return false;
            }
            return true;
        }
    }
}

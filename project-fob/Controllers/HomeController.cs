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
            @ViewBag.title = "Project-fob:";

            return View();
        }

        public ActionResult TestPage()
        {
            return View();
        }

        //Create Meeting
        public ActionResult MeetingPageHost(string attendeePassword, string hostPassword)
        {
            //The password will require different way to send it because atm it is visible

            attendeePassword = attendeePassword ?? "";

            if (hostPassword == null || hostPassword.Length == 0)
            {
                ViewBag.title = "Error: no host password";
                return View("index");
            }
            if (hostPassword.Equals(attendeePassword))
            {
                ViewBag.title = "Error: same passwords";
                return View("index");
            }

            CheckCookies();
            //HttpContext.Session.Set("sessionid", Encoding.ASCII.GetBytes(user.UserId));

            Host host = new Host();
            db.Host.Add(host);

            Meeting meet = new Meeting(GenerateMeetingId(), host, attendeePassword, hostPassword);
            
            while (db.Meeting.Any(m => m.MeetingId.Equals(meet.MeetingId.ToString())))
            {
                meet.MeetingId = GenerateMeetingId();
            }

            db.Meeting.Add(meet);
            HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));

            db.Fob.Add(new Fob(meet));
            db.SaveChanges();

            @ViewBag.title = "Meeting Id: " + meet.MeetingId;
            ViewBag.meetingid = meet.MeetingId;

            return View();
        }

        public ActionResult MeetingPageUser(string meetingId, string password)
        {
            password = password ?? "";

            meetingId = meetingId.ToUpper();
            Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId == meetingId);

            if (meet != null && meetingId.Equals(meet.MeetingId))
            {
                //host
                if (password.Equals(meet.HostPassword))
                {

                    CheckCookies();
                    string id = GetCookieID();
                    //HttpContext.Session.Set("sessionid", Encoding.ASCII.GetBytes(user.UserId));
                    HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));

                    if (meet.Active)
                    {
                        Host host = new Host(id, meet);
                        db.Host.Add(host);

                        db.SaveChanges();
                        return View("MeetingPageHost");
                    }
                    else
                    {
                        return View("~/Views/Home/StatScreen.cshtml");
                    }
                }
                else if (password == meet.RoomPassword && meet.Active)
                {
                    //join as attendee
                    CheckCookies();
                    string id = GetCookieID();
                    
                    HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));
                    
                    ViewBag.title = "Id:" + meetingId;
                    db.SaveChanges();
                    return View();
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

        public static string GenerateGuid()
        {
            Guid g;
            g = Guid.NewGuid();

            return g.ToString();
        }

        public static string GenerateId()
        {
            return GenerateGuid();
        }

        public static string GenerateMeetingId()
        {
            return GenerateUnambiguousMeetingIdByLength(6);
        }

        public static string GenerateUnambiguousMeetingIdByLength(int length)
        {
            Random random = new Random();
            const string chars = "367CDFGHJKMNPRTWX";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void CheckCookies()
        {
            string id = GetCookieID();
            SetCookie(id);
        }

        public string GetID()
        {
            string id = GetCookieID();
            SetCookie(id);
            return id;
        }

        private string GetCookieID()
        {
            return Request.Cookies["ID"];
        }

        private void SetCookie(string id)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id == null || id.Length == 0)
            {
                Response.Cookies.Append("ID", GenerateId());
            }
            else
            {
                Response.Cookies.Append("ID", id);
            }

        }


    }
}

﻿using System;
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
            //The password will require different way to send it because atm it is visible

            attendeePassword = attendeePassword ?? "";
            if (!CheckPasswordsAreCorrectForHosting(attendeePassword,hostPassword))
            {
                ViewBag.title = "Error: Password error";
                return View("index");
            }

            UpdateCookies();


            Meeting meet = MeetingWrapper.CreateMeeting(GenerateMeetingId(), attendeePassword, hostPassword,db);

            while (db.Meeting.Any(m => m.MeetingId.Equals(meet.MeetingId.ToString())))
            {
                meet.MeetingId = GenerateMeetingId();
            }

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
            Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId == meetingId);

            if (meet != null && meetingId.Equals(meet.MeetingId))
            {
                //host
                if (password.Equals(meet.HostPassword))
                {
                    UpdateCookies();
                    var id = Guid.Parse(GetUserIDFromCookie());

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
                else if (password == meet.RoomPassword && meet.Active)
                {
                    //join as attendee
                    UpdateCookies();
                    string id = GetUserIDFromCookie();

                    ViewBag.title = "Id: ";
                    ViewBag.meetingid = meetingId;
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


        public ActionResult UpdateCookies()
        {
            string id = GetUserIDFromCookie();
            SetCookie(id);
            return Ok();
        }

        public string GetUserID()
        {
            string id = GetUserIDFromCookie();
            SetCookie(id);
            return id;
        }

        private static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private static string GenerateMeetingId()
        {
            return GenerateUnambiguousMeetingIdByLength(6);
        }

        private static string GenerateUnambiguousMeetingIdByLength(int length)
        {
            Random random = new Random();
            const string chars = "367CDFGHJKMNPRTWX";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string GetUserIDFromCookie()
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

        private void SetCookie(string id, string value)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddYears(5);
            if (id != null && id.Length == 0)
            {
                Response.Cookies.Append(id, value);
            }
        }


    }
}

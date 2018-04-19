﻿using Microsoft.AspNetCore.Http.Extensions;
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
    public class HostController : Controller
    {
        private readonly ApplicationDbContext db;

        public HostController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QrCode(string meetingIdString)
        {
            string currentUrl = Request.GetDisplayUrl();
            string[] split = currentUrl.Split('/');

            // This variable is really confusing so I'll explain.
            // Baseurl doesn't work because the url can be a long string which I could not use to correctly find the required parameters to enter to the meeting.
            // In case we are running the project on a link like : test.co.uk/project/seesharp/fob/
            // the url request will return something like this: test.co.uk/project/seesharp/fob/index/hostpage. Because of that I have to modify the
            // URL so it grabs the correct url (hopefully).
            // The url editing also needed because of the controll handling it'd generate a wrong url for the user which would render the QRCode useless.

            StringBuilder currentUrlLocationBase = new StringBuilder();

            for (int i = 0; i < split.Length - 2; i++)
            {
                currentUrlLocationBase.Append(split[i]);
            }
            Meeting meet = db.Meeting.Single(x => x.MeetingId.Equals(meetingIdString));

            //The string (url) we generate.
            ViewBag.url = currentUrlLocationBase.ToString() + "/Home/meetingPageUser?meetingId=" + meetingIdString + "&password=" + meet.RoomPassword;
            return View("~/Views/Home/QRCode.cshtml");
        }

        public ActionResult Finish(string message)
        {
            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Single(f => f.Meeting.MeetingId == message);

            fob.Meeting.Stats.Add(new Stats(0, fob.Fobbed.Count, fob.TopicStartTime, DateTime.Now));
            fob.Meeting.Active = false;
            db.SaveChanges();

            ViewBag.meetingid = message;

            //needs to go to the statspage and display the correct stats???

            return View("~/Views/Home/StatScreen.cshtml");
        }
        public ActionResult Leave(string message)
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Refresh(string message, string meetingIdString)
        {
            //update
            if (message != null && message.Length != 0)
            {
                ViewBag.title = message;
            }

            Fob fob = Fob.getFob(meetingIdString, db);

            if (fob == null)
            {
                throw new ArgumentNullException();
            }

            //First number are the total users, the second number is the voted users.
            return Content(fob.Fobbed.Count.ToString());
        }

        public void Reset(string meetingIdString)
        {
            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x=>x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(0, fob.Fobbed.Count, fob.TopicStartTime, DateTime.Now));
            fob.RestartFobbed();

            db.SaveChanges();
        }
    }
}

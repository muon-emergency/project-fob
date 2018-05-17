using Microsoft.AspNetCore.Http.Extensions;
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
            Meeting meet = db.Meeting.Single(x => x.MeetingId.Equals(meetingIdString));

            string url = QrCodeUrlBuilder.BuildUrl(meetingIdString, Request.GetDisplayUrl(), meet.RoomPassword);
            @ViewBag.url = url;
            return View("~/Views/Home/QRCode.cshtml");
        }

        public ActionResult Finish(string message)
        {
            Meeting meeting = MeetingHandler.GetMeetingWithStats(message, db);

            MeetingHandler.FinishMeeting(meeting,db);

            ViewBag.meetingid = message;

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

            Meeting meeting = MeetingHandler.GetMeetingWithFobbed(meetingIdString, db);

            if (meeting == null)
            {
                ViewBag.title = "Error";
                return RedirectToAction("Index");
            }

            //First number are the total users, the second number is the voted users.
            if (meeting.Active)
            {
                return Content(meeting.Fobbed.Count.ToString() + ";" + meeting.TopicCounter);
            }
            else
            {
                return Content("closed");
            }
        }

        public ActionResult Reset(string meetingIdString)
        {
            Meeting meeting = db.Meeting.Include(x => x.Stats)
                            .Include(x => x.Fobbed)
                            .Single(f => f.MeetingId == meetingIdString);

            meeting.Stats.Add(new Stats { Attendeescount = 0, Fobcount = meeting.Fobbed.Count });
            MeetingHandler.RestartFobbed(meeting);

            db.SaveChanges();
            return Ok();
        }
    }
}

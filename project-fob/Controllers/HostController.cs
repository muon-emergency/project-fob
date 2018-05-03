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

            return Content(QrCodeUrlBuilder.BuildUrl(meetingIdString, Request.GetDisplayUrl(), meet.RoomPassword));
        }
        
        public ActionResult Finish(string message)
        {
            Topic fob = db.Topic.Include(x => x.Meeting).ThenInclude(x => x.Stats)
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

            Topic fob = Topic.getFob(meetingIdString, db);

            if (fob == null)
            {
                throw new ArgumentNullException();
            }

            //First number are the total users, the second number is the voted users.
            return Content(fob.Fobbed.Count.ToString());
        }

        public ActionResult Reset(string meetingIdString)
        {
            Topic fob = db.Topic.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x=>x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(0, fob.Fobbed.Count, fob.TopicStartTime, DateTime.Now));
            fob.RestartFobbed();

            db.SaveChanges();
            return Ok();
        }
    }
}

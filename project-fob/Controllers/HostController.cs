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

        public ActionResult QrCode()
        {
            HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);
            
            @ViewBag.url = CreateUrl(meetingIdString);
            return View("~/Views/Home/QRCode.cshtml");
        }

        public string CreateUrl(string meetingIdString)
        {
            Meeting meet = db.Meeting.Single(x => x.MeetingId.Equals(meetingIdString));

            return QrCodeUrlBuilder.BuildUrl(meetingIdString, Request.GetDisplayUrl(), meet.RoomPassword);
        }
        
        public ActionResult Finish(string message)
        {
            HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(0, fob.Fobbed.Count, fob.TopicStartTime, DateTime.Now));
            fob.Meeting.Active = false;
            db.SaveChanges();

            //needs to go to the statspage and display the correct stats???

            return View("~/Views/Home/StatScreen.cshtml");
        }
        public ActionResult Leave(string message)
        {
            return View("~/Views/Home/Index.cshtml");
        }

        public string Refresh(string message)
        {
            //update
            if (message != null && message.Length != 0)
            {
                @ViewBag.title = message;
            }

            HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            Fob fob = Fob.getFob(Encoding.ASCII.GetString(meetingIdValue), db);

            if (fob == null)
            {
                throw new ArgumentNullException();
            }

            //First number are the total users, the second number is the voted users.
            return fob.Fobbed.Count.ToString();
        }

        public void Reset(string message)
        {
            //We need to add the already existing information to the stats model. NAO!!!
            HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(0, fob.Fobbed.Count, fob.TopicStartTime, DateTime.Now));
            fob.RestartFobbed();

            db.SaveChanges();
        }
    }
}

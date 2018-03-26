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
            var gotValue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);
            
            @ViewBag.url = CreateUrl(meetingIdString);
            return View("~/Views/Home/QRCode.cshtml");
        }

        public string CreateUrl(string meetingIdString)
        {
            string baseUrl = Request.GetDisplayUrl();
            string[] split = baseUrl.Split('/');
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < split.Length - 2; i++)
            {
                sb.Append(split[i]);
            }
            Meeting meet = db.Meeting.Single(x => x.MeetingId.Equals(meetingIdString));


            return sb.ToString() + "/Home/meetingPageUser?meetingId=" + meetingIdString + "&password=" + meet.RoomPassword;
        }

        public ActionResult Finish(string message)
        {
            var gotValue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x => x.Meeting).ThenInclude(x => x.Attendee).ThenInclude(x => x.User)
                            .Include(x => x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(fob.Meeting.GetAttendeeCount(), fob.FobCount, fob.TopicStartTime, DateTime.Now));
            fob.Meeting.Active = false;
            db.SaveChanges();

            //needs to go to the statspage and display the correct stats???

            return View("~/Views/Home/StatScreen.cshtml");
        }
        public ActionResult Leave(string message)
        {
            var gotValue = HttpContext.Session.TryGetValue("sessionid", out var session);
            string userIdString = System.Text.Encoding.ASCII.GetString(session);

            //important
            Host host = db.Host
                .Include(x => x.Meeting).ThenInclude(x => x.Host)
                .Include(x => x.User)
                .Single(h => h.User.UserId == userIdString);

            host.Meeting.Host.Remove(host);
            db.SaveChanges();

            // do leave stuff here

            return View("~/Views/Home/Index.cshtml");
        }

        public string Refresh(string message)
        {
            //update
            if (message != null && message.Length != 0)
            {
                @ViewBag.title = message;
            }

            var gotValue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            Fob fob = Fob.getFob(Encoding.ASCII.GetString(meetingIdValue), db);

            if (fob == null)
            {
                throw new ArgumentNullException();
            }

            //First number are the total users, the second number is the voted users.
            return fob.Meeting.GetAttendeeCount() + "," + fob.FobCount;
        }

        public void Reset(string message)
        {
            //We need to add the already existing information to the stats model. NAO!!!
            var gotValue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x => x.Meeting).ThenInclude(x => x.Attendee).ThenInclude(x => x.User)
                            .Include(x => x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == meetingIdString);

            fob.Meeting.Stats.Add(new Stats(fob.Meeting.GetAttendeeCount(), fob.FobCount, fob.TopicStartTime, DateTime.Now));
            fob.RestartFobbed();

            db.SaveChanges();
        }
    }
}

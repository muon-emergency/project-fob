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
            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string byteArrayToString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.fobbed).ThenInclude(x => x.User).Single(f => f.Meeting.MeetingId == byteArrayToString);
            /*Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.fobbed).ThenInclude(x => x.User).SingleOrDefault(f => f.Meeting.MeetingId == byteArrayToString);
            if (fob == null)
            {
                throw new ArgumentNullException();
            }*/
            // if fob.fobbed doesnt contain the current attendee

            gotvalue = HttpContext.Session.TryGetValue("sessionid", out var session);

            string byteArrayToString2 = System.Text.Encoding.ASCII.GetString(session);
            Attendee att = db.Attendee.Include(at => at.User).Include(at => at.Meeting).Single(at => at.User.UserId.Equals(byteArrayToString2) && at.Meeting.MeetingId.Equals(byteArrayToString));


            //Need a rework as if someone is fobbing it it'll not get if the person is already fobbed because of the different primary key for different session.
            //A workaround would be to replace attendee with user as we don't need the attendee anymore as the meeting (or fob) contains the meeting id anyways.
            //bool foundfobber = false;

            /*for (int i = 0; i < fob.fobbed.Count; i++)
            {
                if (fob.fobbed[i].Equals(att))
                {
                    foundfobber = true;
                    i = fob.fobbed.Count;
                }
            }
            */

            bool foundfobber = fob.fobbed.Any(x => x.Equals(att));
            if (!foundfobber)
            {
                fob.FobCount += 1;

                fob.fobbed.Add(att);
                db.SaveChanges();
            }
            //return View("~/Views/Home/MeetingPageUser.cshtml");
        }

        public ActionResult ExitMeeting(string value)
        {
            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string byteArrayToString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x => x.fobbed).ThenInclude(x => x.User).
                            Single(f => f.Meeting.MeetingId == byteArrayToString);

            if (fob.AttendeeCount > 0)
            {
                fob.AttendeeCount -= 1;
            }

            gotvalue = HttpContext.Session.TryGetValue("sessionid", out var sessionBytes);
            var session = Encoding.ASCII.GetString(sessionBytes);

            Attendee att = db.Attendee.SingleOrDefault(f => f.Meeting.MeetingId == session);
            if (fob.fobbed.Contains(att))
            {
                fob.fobbed.Remove(att);

                if (fob.FobCount > 0)
                {
                    fob.FobCount -= 1;
                }
            }
            db.SaveChanges();
            return View("~/Views/Home/Index.cshtml");
        }

        public string ImStillHere()
        {
            /*here we find the attendee in the database and update
             * the attendee's last check in.*/
            return "";
        }
    }
}

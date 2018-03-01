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
            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats).Include(x => x.fobbed).SingleOrDefault(f => f.Meeting.MeetingId == byteArrayToString);
            if (fob == null)
            {
                throw new ArgumentNullException();
            } 
            // if fob.fobbed doesnt contain the current attendee

            gotvalue = HttpContext.Session.TryGetValue("sessionid", out var session);

            string byteArrayToString2 = System.Text.Encoding.ASCII.GetString(session);
            Attendee att = db.Attendee.SingleOrDefault(at => at.User.UserId.Equals((byteArrayToString2)));

            if (att == null) 
            {
                throw new ArgumentNullException();
            }

            if (!fob.fobbed.Contains(att))
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
                            .Include(x => x.fobbed)
                            .SingleOrDefault(f => f.Meeting.MeetingId == byteArrayToString);

            if (fob == null) 
            {
                throw new ArgumentNullException();
            }

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

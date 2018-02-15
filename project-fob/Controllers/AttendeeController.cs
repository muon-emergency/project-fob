using Microsoft.AspNetCore.Mvc;
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
            {
                byte[] meetingIdValue;
                bool gotvalue = false;
                gotvalue = HttpContext.Session.TryGetValue("meetingid", out meetingIdValue);
                Fob fob = Models.Fob.getFob(meetingIdValue.ToString(), db);
                if (fob == null) throw new ArgumentNullException();
                // if fob.fobbed doesnt contain the current attendee


                byte[] session;
                //string session = Session["sessionid"].ToString();
                gotvalue = HttpContext.Session.TryGetValue("sessionid", out session);

                Attendee att = db.Attendee.SingleOrDefault(at => at.User.UserId.Equals(session.ToString()));
                if (att == null) throw new ArgumentNullException();

                if (!fob.fobbed.Contains(att))
                {
                    fob.FobCount += 1;

                    fob.fobbed.Add(att);
                    db.SaveChanges();
                }
                //return View("~/Views/Home/MeetingPageUser.cshtml");
            }
        }

        public ActionResult ExitMeeting(string value)
        {
            {
                byte[] meetingIdValue;
                bool gotvalue = false;
                gotvalue = HttpContext.Session.TryGetValue("meetingid", out meetingIdValue);
                
                //Fob fob = Models.Fob.getFob(Session["meetingid"].ToString(), db);
                Fob fob = Models.Fob.getFob(Encoding.ASCII.GetString(meetingIdValue), db);
                if (fob == null) throw new ArgumentNullException();

                if (fob.AttendeeCount > 0)
                    fob.AttendeeCount -= 1;

                //string sesh = Session["sessionid"].ToString();
                byte[] sessionBytes;
                gotvalue = HttpContext.Session.TryGetValue("sessionid", out sessionBytes);

                var session = Encoding.ASCII.GetString(sessionBytes);

                Attendee att = db.Attendee.SingleOrDefault(at => at.User.UserId.Equals(session));
                if (fob.fobbed.Contains(att))
                {
                    fob.fobbed.Remove(att);

                    if (fob.FobCount > 0) fob.FobCount -= 1;
                }
                db.SaveChanges();
                return View("~/Views/Home/Index.cshtml");
            }
        }


        public string ImStillHere()
        {
            /*here we find the attendee in the database and update
             * the attendee's last check in.*/
            return "";
        }

    }
}

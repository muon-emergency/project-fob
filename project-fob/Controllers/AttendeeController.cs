using project_fob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace project_fob.Controllers
{
    public class AttendeeController : Controller {
        public ActionResult Index() {
            return View();
        }
        public void Fob(string value) {
            using (DAL.FobContext db = new DAL.FobContext()) {
                
                Fob fob = Models.Fob.getFob(Session["meetingid"].ToString(), db);
                if (fob == null) throw new ArgumentNullException();
                // if fob.fobbed doesnt contain the current attendee
                string session = Session["sessionid"].ToString();
                Attendee att = db.Attendee.SingleOrDefault(at => at.User.UserId.Equals(session.ToString()));
                if (att == null) throw new ArgumentNullException();

                if (!fob.fobbed.Contains(att)) {
                    fob.FobCount += 1;
                    
                    fob.fobbed.Add(att);
                    db.SaveChanges();
                }
                //return View("~/Views/Home/MeetingPageUser.cshtml");
            }
        }

        public ActionResult ExitMeeting(string value) {
            using (DAL.FobContext db = new DAL.FobContext()) {

                Fob fob = Models.Fob.getFob(Session["meetingid"].ToString(), db);
                if (fob == null) throw new ArgumentNullException();

                if (fob.AttendeeCount > 0)
                    fob.AttendeeCount -= 1;

                string sesh = Session["sessionid"].ToString();
                Attendee att = db.Attendee.SingleOrDefault(at => at.User.UserId.Equals(sesh));
                if (fob.fobbed.Contains(att)) {
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

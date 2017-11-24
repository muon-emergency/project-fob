using project_fob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//http://localhost:56403/Home/About?id=1 an example how to send parameters
//http://localhost:56403/Home/About?id=1&admin=true for 2 parameters

namespace project_fob.Controllers
{
    public class HomeController : Controller{
        
        //Current SESSION
        //do a get meeting from id from a static meeting store?
        public ActionResult Index(string message)
        {
            @ViewBag.title = "Project-fob:";

            return View();
        }

        //Create Meeting
        public ActionResult meetingPageHost(string attendeePassword, string hostPassword) { //The password will require different way to send it because atm it is visible
            DAL.FobContext db = new DAL.FobContext();

            User user = new User();
            if (Session["sessionid"] == null || Session["sessionid"].ToString().Length < 9) {
                Session["sessionid"] = user.UserId;
            }

            db.User.Add(user);
            db.SaveChanges();
            db.Host.Add(new Host(db.User.Where(u => u.UserId == user.UserId).SingleOrDefault()));
            db.SaveChanges();

            Meeting meet = new Meeting(db.Host.Where(h=> h.User.UserId == user.UserId).SingleOrDefault(),attendeePassword,hostPassword);
            db.Meeting.Add(meet);
            db.SaveChanges();

            db.Fob.Add(new Fob(meet));
            db.SaveChanges();

            @ViewBag.title = "Host page\n Meeting Id: " + meet.MeetingId;
            return View();
        }

       
        //Attendee Page
        public ActionResult meetingPageUser(string meetingId, string password) { //The password will require different way to send it because atm it is visible
            //get meeting from database that matches the Id
            DAL.FobContext db = new DAL.FobContext();
            Meeting meet = db.Meeting.Where(m => m.MeetingId == meetingId && m.Active == true).SingleOrDefault();
            if (meet != null && meetingId.Equals(meet.MeetingId)) {
                User user = new User();
                if (Session["sessionid"] == null || Session["sessionid"].ToString().Length < 9) {
                    Session["sessionid"] = user.UserId;
                }
                db.User.Add(user);
                db.SaveChanges();

                db.Attendee.Add(new Attendee(db.User.Where(u => u.UserId == user.UserId).SingleOrDefault(), meet));
                db.SaveChanges();
                
                ViewBag.Message = "Meeting id:" + meetingId;

                return View();
            }
            ViewBag.title = "Meeting Does Not Exist. Please Try Again";
            return View("Index");
        }

       
        public static string generateId() {
            return generateId(9);
        }
        //change to use ascii ?
        public static string generateId(int length) { 
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult TestPage()
        {
            return View();
        }
    }


}
 
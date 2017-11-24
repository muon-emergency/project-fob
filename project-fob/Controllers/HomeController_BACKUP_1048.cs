using project_fob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
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

<<<<<<< HEAD
        public ActionResult meetingPageHost(string userPassword, string hostPassword) //The password will require different way to send it because atm it is visible
        {
            if (hostPassword == null)
            {
                return View("Index");
            }

            ViewBag.Button = "Gtfo pls";

            User user = new User() { UserId = "111" };
           // users.Add(user);
            host = new Host() { User = user, UserId = user.UserId };


            meet = new Meeting() { MeetingId = id,
                RoomPassword = userPassword,
                HostPassword = hostPassword, Host = new List<Host>() { host }
            };
            host.Meeting = meet;
            host.MeetingId = meet.MeetingId;


            /*run a query which returns if the meeting page is created. If it`s done 
             *grab the meetingRoomId
             * */

            //ViewBag.Title = "Host page\n Meeting Id: " + id;
            ViewBag.Title = "Meeting Id: " + id;
            

            //ViewBag.Message = "Host rights\nRoom id:" + id;
=======
        //Create Meeting
        public ActionResult meetingPageHost(string attendeePassword, string hostPassword) { //The password will require different way to send it because atm it is visible
            DAL.FobContext db = new DAL.FobContext();

            User user = new User();
            if (Session["sessionid"] == null || Session["sessionid"].ToString().Length < 9) {
                Session["sessionid"] = user.UserId;
            }
>>>>>>> ChrisDatabase

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

        public ActionResult StatScreen()
        {
            return View();
        }
        

        public ActionResult TestPage()
        {
            return View();
        }

        public string MeetingQuery()
        {
            Random r = new Random();
            int randomNumber1 = r.Next(10000);
            int randomNumber2 = r.Next(randomNumber1);
            return randomNumber1+","+randomNumber2; //First number are the total users, the second number is the voted users.
        }
    }


}
 
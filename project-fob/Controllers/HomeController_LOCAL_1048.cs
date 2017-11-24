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
        //TEMP FOR DEMO
        public static Meeting meet;
        public static Host host;
        public static List<User> users;
        public static string id = "123456789";
        public static  List<Attendee> attendees;
        public static List<string> Fobbed;
        public static int fobcount;
        public static int attendeecount;

        public ActionResult Index(string message)
        {
            users = new List<User>();
            attendees = new List<Attendee>();
            
            if (message!=null&& message.Length != 0)
            {
                @ViewBag.title = message;
            }
            else
            {
                @ViewBag.title = "Project-fob:";
            }

            if (Session["user"] == null || Session["user"].ToString().Length < 9){
                //generate userId
            }
            return View();
        }

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


            //Insert sql query for asking for the meeting room or if the ID doesn`t exist create one

            /* we display different functions like how many user is in the meeting
             * current topic, current presenter, current presenter's percentage that how many user clicked on the button
             * in the meeting.*/

            return View();
        }

        //public ActionResult meetingPageUser(int id, string password) //The password will require different way to send it because atm it is visible
        public ActionResult meetingPageUser(string meetingId, string password) //The password will require different way to send it because atm it is visible
        {
            if (meetingId == null)
            {
                return View("Index");
            }
            if(meetingId.Equals(id.ToLower())) {
                
                User user = new User() { UserId = "222" };
               // users.Add(user);

                Attendee att = new Attendee() { Meeting = meet, MeetingId = meet.MeetingId, User = user };
                // attendees.Add(att);
                attendeecount++; // remove on disconnect
            }
            /*we display functions like current topic, # of presenter in topic and the most
             * important is the big shiny button.
             */

            /* We need to search for the id first. If the id is found we need to check if the 
             userpassword or the host password is equal with the typed in password by the user*/

            //If everything goes right:
            ViewBag.Message = "Meeting id:" + meetingId;

            return View();
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
 
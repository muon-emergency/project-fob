using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using project_fob.Models;
using System.Text;
using project_fob.Data;
using Microsoft.EntityFrameworkCore;

namespace project_fob.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            @ViewBag.title = "Project-fob:";

            return View();
        }

        public ActionResult TestPage()
        {
            return View();
        }
        /*
				public ActionResult StatScreen()
				{
					return View();
				}
				*/

        //Create Meeting
        public ActionResult meetingPageHost(string attendeePassword, string hostPassword)
        { //The password will require different way to send it because atm it is visible

            attendeePassword = attendeePassword ?? "";

            if (hostPassword == null || hostPassword.Length == 0)
            {
                ViewBag.title = "Error: no host password";
                return View("index");
            }
            if (hostPassword.Equals(attendeePassword))
            {
                ViewBag.title = "Error: same passwords";
                return View("index");
            }

            User user = new User(generateId());
            while (db.User.Any(m => m.UserId.Equals(user.UserId)))
            {
                user.UserId = generateId();
            }
            db.User.Add(user);

            HttpContext.Session.Set("sessionid", Encoding.ASCII.GetBytes(user.UserId));
            //Session["sessionid"] = user.UserId;

            Host host = new Host(user);
            db.Host.Add(host);

            Meeting meet = new Meeting(generateId(), host, attendeePassword, hostPassword);
            //This might cause some incidents in case we generate 2 rooms with the same ID
            while (db.Meeting.Any(m => m.MeetingId.Equals(meet.MeetingId.ToString()) && m.Active))
            {
                meet.MeetingId = generateId();
            }

            db.Meeting.Add(meet);
            HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));
            //Session["meetingid"] = meet.MeetingId;

            db.Fob.Add(new Fob(meet));
            db.SaveChanges();

            @ViewBag.title = "Meeting Id: " + meet.MeetingId;
            //TODO generae QR code for the meeting and display it on its own page (add something to host to get this if lost)

            return View();
        }


        //Join Meeting                                          //will this string be empty or null ?
        public ActionResult meetingPageUser(string meetingId, string password)
        { //The password will require different way to send it because atm it is visible
            {
                if (password == null)
                {
                    password = "";
                }
                //Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId == meetingId && m.Active);
                Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId == meetingId);

                if (meet != null && meetingId.Equals(meet.MeetingId))
                {
                    //host
                    if (password.Equals(meet.HostPassword.ToString()))
                    {
                        
                            User user = new User(generateId());
                            while (db.User.Any(m => m.UserId.Equals(user.UserId)))
                            {
                                user.UserId = generateId();
                            }
                            db.User.Add(user);

                            HttpContext.Session.Set("sessionid", Encoding.ASCII.GetBytes(user.UserId));
                            HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));
                        //Session["sessionid"] = user.UserId;
                        //Session["meetingid"] = meet.MeetingId;

                        if (meet.Active)
                        {
                            Host host = new Host(user, meet);
                            db.Host.Add(host);

                            db.SaveChanges();
                            return View("MeetingPageHost");
                        }
                        else
                        {
                            return View("~/Views/Home/StatScreen.cshtml");
                        }
                    }
                    else if (password == meet.RoomPassword && meet.Active)
                    {
                        //join as attendee
                        User user = new User(generateId());
                        while (db.User.Any(m => m.UserId.Equals(user.UserId)))
                        {
                            user.UserId = generateId();
                        }

                        HttpContext.Session.Set("sessionid", Encoding.ASCII.GetBytes(user.UserId));
                        HttpContext.Session.Set("meetingid", Encoding.ASCII.GetBytes(meet.MeetingId));

                        //Session["sessionid"] = user.UserId;
                        //Session["meetingid"] = meet.MeetingId;
                        db.User.Add(user);
                        Attendee att = new Attendee(user, meet);
                        //db.Attendee.Add(att);

                        //Not working solution atm, but the direction is correct.
                        /*Attendee foundAttendee = db.Attendee.Include(x => x.Meeting).Include(x => x.User).SingleOrDefault(f => f.Meeting.MeetingId == meet.MeetingId && f.Meeting.Attendee.Equals(user.UserId));
                        
                        //if attendee is not in then we add him.
                        if (foundAttendee == null)
                        {
                            db.Attendee.Add(att);
                        }*/

                        db.Attendee.Add(att);
                        meet.Attendee.Add(att);

                        Fob fob = db.Fob.SingleOrDefault(f => f.Meeting == db.Meeting.FirstOrDefault(m => m.MeetingId.Equals(meetingId) && m.Active));
                        if (fob == null)
                        {
                            throw new ArgumentNullException();
                        }

                        fob.AttendeeCount += 1;

                        ViewBag.title = "Id:" + meetingId;

                        db.SaveChanges();
                        return View();
                    }
                    else
                    {
                        //meeting pass incorrect
                        ViewBag.title = "Meeting Password Incorrect. Please Try Again";
                        return View("Index");
                    }
                }
                ViewBag.title = "Meeting Does Not Exist. Please Try Again";

                return View("Index");
            }
        }

        //open new tab?
        public ActionResult returnGeneratedQrCode()
        {
            /*using (DAL.FobContext db = new DAL.FobContext())
            {
                string meetingId = Session["meetingId"].ToString();
                string userId = Session["sessionId"].ToString();
                if (meetingId != null && userId != null)
                {
                    Host host = db.Host.FirstOrDefault(h => h.User.UserId.Equals(userId));
                    Meeting meet = db.Meeting.FirstOrDefault(m => m.MeetingId.Equals(meetingId) && m.Active);
                    if (meet.Host.Contains(host))
                    {
                        Bitmap qrCode = generateQrCode(meetingId, meet.RoomPassword.ToString());

                        byte[] byteArray = ImageToByte(qrCode);
                        return File(byteArray, "image/jpeg");
                    }
                }*/
            return null;
        }

        public static string generateId()
        {
            return generateIdByLength(9);
        }
        //change to use ascii ?
        public static string generateIdByLength(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /*public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        //http://localhost:56403/Home/meetingPageUser?meetingId=G1YC4BAHI&password=hi
        public Bitmap generateQrCode(string meetingId, string password)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(baseUrl + "/Home/meetingPageUser?meetingId=" + meetingId + "&password=" + password, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }*/



        /*public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}

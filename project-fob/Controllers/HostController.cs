﻿using Microsoft.AspNetCore.Mvc;
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
        public ActionResult Finish(string message)
        {
            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            string byteArrayToString = System.Text.Encoding.ASCII.GetString(meetingIdValue);
            
            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x => x.Meeting).ThenInclude(x => x.Attendee).ThenInclude(x => x.User)
                            .Include(x => x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == byteArrayToString);

            fob.Meeting.Stats.Add(new Stats(fob.Meeting.GetAttendeeCount(), fob.FobCount, fob.TopicStartTime, DateTime.Now));
            fob.Meeting.Active = false;
            db.SaveChanges();

            //needs to go to the statspage and display the correct stats???

            return View("~/Views/Home/StatScreen.cshtml");
        }
        public ActionResult Leave(string message)
        {
            var gotvalue = HttpContext.Session.TryGetValue("sessionid", out var session);
            string byteArrayToString = System.Text.Encoding.ASCII.GetString(session);

            //important
            Host host = db.Host
                .Include(x => x.Meeting).ThenInclude(x => x.Host)
                .Include(x => x.User)
                .Single(h => h.User.UserId == byteArrayToString);

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

            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);
            Fob fob = Fob.getFob(Encoding.ASCII.GetString(meetingIdValue), db);

            if (fob == null)
            {
                throw new ArgumentNullException();
            }

            //First number are the total users, the second number is the voted users.
            return fob.Meeting.GetAttendeeCount() + "," + fob.FobCount;
        }

        /*public ActionResult returnGeneratedQrCode()
        {
            Bitmap qrCode = generateQrCode();

            byte[] byteArray = ImageToByte(qrCode);
            return File(byteArray, "image/jpeg");
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public Bitmap generateQrCode()
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("hello", QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }
        */
        public void Reset(string message)
        {
            //We need to add the already existing information to the stats model. NAO!!!
            var gotvalue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string byteArrayToString = System.Text.Encoding.ASCII.GetString(meetingIdValue);
            
            Fob fob = db.Fob.Include(x => x.Meeting).ThenInclude(x => x.Stats)
                            .Include(x => x.Meeting).ThenInclude(x => x.Attendee).ThenInclude(x => x.User)
                            .Include(x => x.Fobbed)
                            .Single(f => f.Meeting.MeetingId == byteArrayToString);

            fob.Meeting.Stats.Add(new Stats(fob.Meeting.GetAttendeeCount(), fob.FobCount, fob.TopicStartTime, DateTime.Now));
            fob.RestartFobbed();
            
            db.SaveChanges();
        }
    }
}

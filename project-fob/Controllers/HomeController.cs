using project_fob.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Linq;
using System.Web.Mvc;

namespace project_fob.Controllers
{
    public class HomeController : Controller{

		//Current SESSION
		//do a get meeting from id from a static meeting store?
		public ActionResult Index(string message) {
			@ViewBag.title = "Project-fob:";

			return View();
		}

		public ActionResult TestPage() {
			return View();
		}
		/*
				public ActionResult StatScreen()
				{
					return View();
				}
				*/




		//Create Meeting
		public ActionResult meetingPageHost(string attendeePassword, string hostPassword) { //The password will require different way to send it because atm it is visible

			if (hostPassword == null || hostPassword.Length == 0) {
				ViewBag.title = "Error: no host password";
				return View("index");
			}
			if (hostPassword.Equals(attendeePassword)) {
				ViewBag.title = "Error: same passwords";
				return View("index");
			}

			using (DAL.FobContext db = new DAL.FobContext()) {

				User user = new User(generateId());
				while (db.User.Any(m => m.UserId.Equals(user.UserId))) {
					user.UserId = generateId();
				}
				db.User.Add(user);
				Session["sessionid"] = user.UserId;

				Host host = new Host(user);
				db.Host.Add(host);

				Meeting meet = new Meeting(generateId(), host, attendeePassword, hostPassword);
				while (db.Meeting.Any(m => m.MeetingId.Equals(meet.MeetingId.ToString()) && m.Active)) {
					meet.MeetingId = generateId();
				}

				db.Meeting.Add(meet);
				Session["meetingid"] = meet.MeetingId;


				db.Fob.Add(new Fob(meet));
				db.SaveChanges();

				@ViewBag.title = "Meeting Id: " + meet.MeetingId;
				//TODO generae QR code for the meeting and display it on its own page (add something to host to get this if lost)

				return View();
			}
		}


		//Join Meeting                                          //will this string be empty or null ?
		public ActionResult meetingPageUser(string meetingId, string password) { //The password will require different way to send it because atm it is visible
			using (DAL.FobContext db = new DAL.FobContext()) {
				if (password == null) {
					password = "";
				}
				Meeting meet = db.Meeting.SingleOrDefault(m => m.MeetingId.Equals(meetingId) && m.Active);

				if (meet != null && meetingId.Equals(meet.MeetingId)) {
					//host
					if (password.Equals(meet.HostPassword.ToString())) {

						User user = new User(generateId());
						while (db.User.Any(m => m.UserId.Equals(user.UserId))) {
							user.UserId = generateId();
						}
						db.User.Add(user);

						Session["sessionid"] = user.UserId;
						Session["meetingid"] = meet.MeetingId;

						Host host = new Host(user, meet);
						db.Host.Add(host);

						db.SaveChanges();
						return View("MeetingPageHost");

					} else if (password.ToString().Equals(meet.RoomPassword.ToString())) {
						//join as attendee
						User user = new User(generateId());
						while (db.User.Any(m => m.UserId.Equals(user.UserId))) {
							user.UserId = generateId();
						}

						Session["sessionid"] = user.UserId;
						Session["meetingid"] = meet.MeetingId;
						db.User.Add(user);
						Attendee att = new Attendee(user, meet);
						db.Attendee.Add(att);


						meet.Attendee.Add(att);

						Fob fob = db.Fob.SingleOrDefault(f => f.Meeting == db.Meeting.FirstOrDefault(m => m.MeetingId.Equals(meetingId) && m.Active));
						if (fob == null) throw new ArgumentNullException();

						fob.AttendeeCount += 1;

						ViewBag.title = "Id:" + meetingId;

						db.SaveChanges();
						return View();
					} else {
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
		public ActionResult returnGeneratedQrCode() {
			using (DAL.FobContext db = new DAL.FobContext()) {
				string meetingId = Session["meetingId"].ToString();
				string userId = Session["sessionId"].ToString();
				if (meetingId != null && userId != null) {
					Host host = db.Host.FirstOrDefault(h => h.User.UserId.Equals(userId));
					Meeting meet = db.Meeting.FirstOrDefault(m => m.MeetingId.Equals(meetingId) && m.Active);
					if (meet.Host.Contains(host)) {
						Bitmap qrCode = generateQrCode(meetingId, meet.RoomPassword.ToString());

						byte[] byteArray = ImageToByte(qrCode);
						return File(byteArray, "image/jpeg");
					}
				}
				return null;
			}
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

		public static byte[] ImageToByte(Image img) {
			ImageConverter converter = new ImageConverter();
			return (byte[])converter.ConvertTo(img, typeof(byte[]));
		}
		//http://localhost:56403/Home/meetingPageUser?meetingId=G1YC4BAHI&password=hi
		public Bitmap generateQrCode(string meetingId, string password) {
			QRCodeGenerator qrGenerator = new QRCodeGenerator();
			string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
			QRCodeData qrCodeData = qrGenerator.CreateQrCode(baseUrl + "/Home/meetingPageUser?meetingId=" + meetingId + "&password=" + password, QRCodeGenerator.ECCLevel.Q);
			QRCode qrCode = new QRCode(qrCodeData);
			return qrCode.GetGraphic(20);
		}
	}
}


using project_fob.DAL;
using project_fob.Models;
using QRCoder;
using System.Drawing;
using System;
using System.Linq;
using System.Web.Mvc;

namespace project_fob.Controllers
{
    public class HostController : Controller {
        
        public ActionResult Index(){
            return View();
        }
        public ActionResult Finish(string message) {
            using (DAL.FobContext db = new DAL.FobContext()) {
                
                Fob fob = Fob.getFob(Session["meetingid"].ToString(), db);
                if (fob == null) throw new ArgumentNullException();
                fob.Meeting.Stats.Add(new Stats(fob.AttendeeCount, fob.FobCount, fob.TopicStartTime, DateTime.Now));
                fob.Meeting.Active = false;
                db.SaveChanges();
                
                //needs to go to the statspage and display the correct stats???

            return View("~/Views/Home/StatScreen.cshtml");
            }
        }
        public ActionResult Leave(string message) {
            using (FobContext db = new FobContext()) {
                string session = Session["sessionid"].ToString();

                Host host = db.Host.SingleOrDefault(h => h.User.UserId.Equals(session.ToString()));
                if (host == null) throw new ArgumentNullException();
                host.Meeting.Host.Remove(host);
                db.SaveChanges();
                
                // do leave stuff here

                return View("~/Views/Home/Index.cshtml");
            }
        }
        public string Refresh(string message) {
            using (FobContext db = new FobContext()) {
                //update
                if (message != null && message.Length != 0) {
                    @ViewBag.title = message;
                }
                Fob fob = Fob.getFob(Session["meetingid"].ToString(), db);
                if (fob == null) throw new ArgumentNullException();

                //First number are the total users, the second number is the voted users.
                return fob.AttendeeCount + "," + fob.FobCount;
               
            }
        }

        public ActionResult returnGeneratedQrCode()
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

        public void Reset(string message) {

            //We need to add the already existing information to the stats model. NAO!!!

            using (FobContext db = new FobContext()) {
                Fob fob = Fob.getFob(Session["meetingid"].ToString(), db);
                if (fob == null) throw new ArgumentNullException();


                fob.Meeting.Stats.Add(new Stats(fob.AttendeeCount, fob.FobCount, fob.TopicStartTime, DateTime.Now));
                
                fob.FobCount = 0;
                fob.fobbed.Clear();
                db.SaveChanges();
            }
        }


    }

}

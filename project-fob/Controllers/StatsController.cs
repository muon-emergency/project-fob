using Microsoft.AspNetCore.Mvc;
using project_fob.Data;
using project_fob.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_fob.Controllers
{
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext db;

        public StatsController(ApplicationDbContext db)
        {
            this.db = db;
        }
        // GET: Stats
        public ActionResult Index()
        {
            return View();
        }
        public string getStats()
        {
            { //- for in the same topic, : for different topic, ; for completely different statistic

                byte[] meetingIdValue;
                bool gotvalue = false;
                gotvalue = HttpContext.Session.TryGetValue("meetingid", out meetingIdValue);

                string session = meetingIdValue.ToString();
                //string session = Session["meetingid"].ToString();
                List<Stats> stats = db.Meeting.FirstOrDefault(m => m.MeetingId.Equals(session.ToString())).Stats;

                /*List<Stats> stats= new List<Stats>();

                stats.Add(new Stats(5, 2, DateTime.Now, DateTime.Now));
                stats.Add(new Stats(15, 8, DateTime.Now, DateTime.Now));
                stats.Add(new Stats(32, 5, DateTime.Now, DateTime.Now));*/


                String tmp = "";

                int totalAtt = 0;
                int totalFob = 0;
                for (int i = 0; i < stats.Count; i++)
                {
                    totalAtt += stats[i].Attendeescount;
                    totalFob += stats[i].Fobcount;
                    tmp += stats[i].Attendeescount + "-" + stats[i].Fobcount + ":";
                }
                tmp += ";" + totalAtt / (double)stats.Count + "-" + totalFob / (double)stats.Count;



                //TODO use these stats (they should be in order as a list is deterministic)

                return tmp;

            }
        }
    }
}

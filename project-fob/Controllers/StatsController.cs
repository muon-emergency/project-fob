using Microsoft.AspNetCore.Mvc;
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
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext db;

        public StatsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        public string GetStats()
        { //- for in the same topic, : for different topic, ; for completely different statistic

            bool gotValue = false;
            gotValue = HttpContext.Session.TryGetValue("meetingid", out var meetingIdValue);

            string session = meetingIdValue.ToString();
            string meetingIdString = System.Text.Encoding.ASCII.GetString(meetingIdValue);

            Meeting meeting = db.Meeting.Include(x => x.Stats).Single(m => m.MeetingId == meetingIdString);

            List<Stats> stats = meeting.Stats;
            StringBuilder sb = new StringBuilder();

            int totalAtt = 0;
            int totalFob = 0;
            for (int i = 0; i < stats.Count; i++)
            {
                totalAtt += stats[i].Attendeescount;
                totalFob += stats[i].Fobcount;
                sb.Append(stats[i].Attendeescount + "-" + stats[i].Fobcount + ":");
            }
            sb.Append(";" + totalAtt / (double)stats.Count + "-" + totalFob / (double)stats.Count);

            //TODO use these stats (they should be in order as a list is deterministic)

            return sb.ToString();

        }
    }
}

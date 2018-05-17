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

        public string GetStats(string meetingIdString)
        { //- for in the same topic, : for different topic, ; for completely different statistic

            Meeting meeting = MeetingHandler.GetMeetingWithStats(meetingIdString, db);

            List<Stats> stats = meeting.Stats;
            StringBuilder sb = new StringBuilder();
            
            int totalFob = 0;
            for (int i = 0; i < stats.Count; i++)
            {
                totalFob += stats[i].Fobcount;
                sb.Append(stats[i].Fobcount + ":");
            }
            sb.Append(";" + totalFob / (double)stats.Count);

            return sb.ToString();

        }
    }
}

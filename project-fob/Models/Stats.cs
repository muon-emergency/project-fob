using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace project_fob.Models
{
    public class StatsClick
    {
        public int Id { get; set; }
        public DateTime ClickTime { get; set; }
    }

    public class Stats
    {
        [Key]
        public int Id { get; set; }
        public int Attendeescount { get; set; }
        public int Fobcount { get; set; }
        public DateTime TopicStartTime { get; set; }
        public DateTime TopicStopTime { get; set; }
        public List<StatsClick> Clicks { get; set; }

        public Stats() { }
        public Stats(int attendeesCount, int fobcount, DateTime topicStartTime, DateTime topicStopTime, List<DateTime> clicks)
        {
            Attendeescount = attendeesCount;
            Fobcount = fobcount;
            TopicStartTime = topicStartTime;
            TopicStopTime = topicStopTime;
            Clicks = clicks.Select(x => new StatsClick { ClickTime = x }).ToList();
        }
        public Stats(int attendeesCount, int fobcount, DateTime topicStartTime, DateTime topicStopTime)
        {
            Attendeescount = attendeesCount;
            Fobcount = fobcount;
            TopicStartTime = topicStartTime;
            TopicStopTime = topicStopTime;
        }
    }
}

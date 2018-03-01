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
        public int Attendeescount { get; }
        public int Fobcount { get; }
        public DateTime TopicStartTime { get; }
        public List<StatsClick> Clicks { get; }
        public DateTime TopicStopTime { get; }

        public Stats() { }
        public Stats(int attendeesCount,int fobcount, DateTime topicStartTime, DateTime topicStopTime, List<DateTime> clicks) 
        {
            this.Attendeescount = attendeesCount;
            this.Fobcount = fobcount;
            this.TopicStartTime = topicStartTime;
            this.TopicStopTime = topicStopTime;
            this.Clicks = clicks.Select(x => new StatsClick { ClickTime = x }).ToList();
        }
        public Stats(int attendeesCount, int fobcount, DateTime topicStartTime, DateTime topicStopTime) 
        {
            this.Attendeescount = attendeesCount;
            this.Fobcount = fobcount;
            this.TopicStartTime = topicStartTime;
            this.TopicStopTime = topicStopTime;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace project_fob.Models{
    
    public class Stats{

        [Key]
        public int Id { get; set; }
        public int Attendeescount { get; private set; }
        public int Fobcount { get; private set; }
        public DateTime TopicStartTime { get; private set; }
        public List<DateTime> Clicks { get; private set; }
        public DateTime TopicStopTime { get; private set; }

        public Stats() { }
        public Stats(int attendeesCount,int fobcount, DateTime topicStartTime, DateTime topicStopTime, List<DateTime> clicks) {
            this.Attendeescount = attendeesCount;
            this.Fobcount = fobcount;
            this.TopicStartTime = topicStartTime;
            this.TopicStopTime = topicStopTime;
            this.Clicks = clicks;

        }
        public Stats(int attendeesCount, int fobcount, DateTime topicStartTime, DateTime topicStopTime) {
            this.Attendeescount = attendeesCount;
            this.Fobcount = fobcount;
            this.TopicStartTime = topicStartTime;
            this.TopicStopTime = topicStopTime;

        }

    }
}
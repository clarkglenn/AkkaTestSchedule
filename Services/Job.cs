using System;

namespace AkkaTestSchedule.Services
{
    public class Job
    {
        public DateTime DateTime { get; set; }
        public int TaskId { get; set; }
        public bool Fail { get; set; }
    }
}
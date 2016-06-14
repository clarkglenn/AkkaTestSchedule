using System;
using System.Collections.Generic;
using System.Linq;
using Akka.Util.Internal;

namespace AkkaTestSchedule.Services
{
    public class JobRepository
    {
        private bool isRun = false;
        public IEnumerable<Job> GetTasks()
        {
            if (!isRun)
            {
                isRun = true;
                return Enumerable.Range(0, 10).Select(i => 
                    new Job() { DateTime = DateTime.Now.AddMinutes(i), TaskId = i, Fail = i % 2 == 0 }
                );
            }

            return new List<Job>();
        }
    }
}
using System;
using Akka.Actor;
using AkkaTestSchedule.Services;

namespace AkkaTestSchedule.Actors
{
    public class ExceuteScheduledTaskActor : ReceiveActor
    {
        public ExceuteScheduledTaskActor()
        {
            Receive<Job>(task =>
            {
                ExecuteTask(task, 0);
            });

            Receive<RetryScheduledTask>(task =>
            {
                if(task.FailureCount < 3)
                    ExecuteTask(task.ScheduledTask, task.FailureCount);
                else
                {
                    Console.WriteLine("TaskId: {0} run 3 times, ignoring.", task.ScheduledTask.TaskId);
                }
            });
        }

        private void ExecuteTask(Job task, int failureCount)
        {
            //Simulate error
            if (task.Fail)
            {
                //Try something
                //When fail send it to the ReScheduleTaskActor();
                failureCount = failureCount + 1;
                var retryDelay = TimeSpan.FromSeconds(failureCount*1);
                var retry = new RetryScheduledTask(task, 10, failureCount);
                Console.WriteLine("TaskId: {0} run badly, retrying in {1} seconds", task.TaskId, retryDelay.TotalSeconds);

                //Reschedule
                Context.System.Scheduler.ScheduleTellOnce(retryDelay, Self, retry, Sender);
            }
            else
            {
                Console.WriteLine("TaskId: {0} run ok", task.TaskId);
            }
        }

        protected override void PostStop()
        {
            Console.WriteLine("This one is stopped.");
        }

        public class RetryScheduledTask
        {
            public RetryScheduledTask(Job task, int ttl, int failureCount)
            {
                ScheduledTask = task;
                Ttl = ttl;
                FailureCount = failureCount;
            }

            public int Ttl { get; private set; }
            public Job ScheduledTask { get; private set; }
            public int FailureCount { get; set; }
        }
    }
}
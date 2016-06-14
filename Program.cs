using System;
using System.Threading.Tasks;
using Akka.Actor;
using AkkaTestSchedule.Actors;
using AkkaTestSchedule.Services;

namespace AkkaTestSchedule
{
    class Program
    {
        static void Main(string[] args)
        {
            var actorSystem = ActorSystem.Create("TestActorSystem");
            var pollTasksActorProps = actorSystem.ActorOf(Props.Create(() => new JobQueuePollingActor(new JobRepository())), "PollJobQueueActor");
            
            //Check for a scheduled task every 5 seconds.
            actorSystem.
                Scheduler.
                ScheduleTellRepeatedly(TimeSpan.Zero, TimeSpan.FromSeconds(5), pollTasksActorProps, new StartPollingTaskMessage(), ActorRefs.Nobody);

            actorSystem.AwaitTermination();
        }
    }
}

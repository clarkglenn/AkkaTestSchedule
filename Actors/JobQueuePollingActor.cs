using System;
using System.Runtime.CompilerServices;
using Akka.Actor;
using Akka.Routing;
using Akka.Util.Internal;
using AkkaTestSchedule.Services;

namespace AkkaTestSchedule.Actors
{
    public class JobQueuePollingActor : ReceiveActor
    {
        private readonly JobRepository _jobRepository;
        private IActorRef _taskRunner;

        protected override void PreStart()
        {
            var poolRoute = new RoundRobinPool(2);
            _taskRunner = Context.ActorOf(Props.Create(() => new ExceuteScheduledTaskActor()).WithRouter(poolRoute), this.GetType().ToString());
        }
        
        public JobQueuePollingActor(JobRepository jobRepository)
        {
            _jobRepository = jobRepository;
            Receive<StartPollingTaskMessage>(e =>
            {
                var tasks = _jobRepository.GetTasks();
                tasks.ForEach(t => _taskRunner.Tell(t));
                //Fetch tasks that is not yet run and also older than DateTime.Now
                //Fetch tasks that should be run during the next 10 minutes.
                Console.WriteLine("Polling for tasks to schedule the next hour");
            });
        }
    }

    public class StartPollingTaskMessage
    {
    }
}
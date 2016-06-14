using System;
using System.Runtime.CompilerServices;
using Akka.Actor;
using Akka.Routing;
using Akka.Util.Internal;
using AkkaTestSchedule.Services;

namespace AkkaTestSchedule.Actors
{
    public class MarkTaskRunActor : ReceiveActor
    {
        protected override void PreStart()
        {
            
        }

        public MarkTaskRunActor(JobRepository jobRepository)
        {
            
        }
    }
}
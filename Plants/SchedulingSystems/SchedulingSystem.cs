using System;
using System.Collections.Generic;
using System.Linq;
using Planting.MeasuringsProviding;
using Planting.Observing;

namespace Planting.SchedulingSystems
{
    public class SchedulingSystem : IReciever
    {
        public ISender<SchedulingMessage> Sender { get; private set; }
        public IList<Schedule> Schedules { get; private set; }

        public SchedulingSystem(ISender<SchedulingMessage> sender, IList<Schedule> schedules)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            Schedules = schedules;
        }

        public SchedulingSystem(ISender<SchedulingMessage> sender)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            Schedules = new List<Schedule>();
        }

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<SchedulingMessage> messengingEventArgs = eventArgs as MessengingEventArgs<SchedulingMessage>;
            if (messengingEventArgs != null)
            {
                SchedulingMessage recievedMessage = messengingEventArgs.Object;
                Console.WriteLine("Message is accepted (SchedulingSystem)!");
            }
        }

        public Schedule CreateSchedule(SchedulingMessage message)
        {
            Schedule schedule = Schedules.First(source => source.PlantsAreaId == message.PlantsAreaId);
         }
    }
}

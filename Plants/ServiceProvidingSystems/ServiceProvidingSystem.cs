using System;
using System.Collections.Generic;
using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.Observation;

namespace Planting.ServiceProvidingSystems
{
    public abstract class ServiceProvidingSystem : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }

        protected ServiceProvidingSystem(ISender<MeasuringMessage> sender)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;
        }
        
        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<MeasuringMessage> messengingEventArgs = eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
                Console.WriteLine("Message is accepted (SchedulingSystem)!\n");
            }
        }

        public virtual void StartService()
        {
            // service plants
        }
    }
}

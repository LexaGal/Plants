using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Planting.MeasuringsProviding;
using Planting.MessagesCreators;
using Planting.PlantRequirements;
using Planting.Plants;
using Planting.SchedulingSystems;

namespace Planting.Observing
{
    public class Observer : IReciever, ISender<SchedulingMessage>
    {
        public ISender<MeasuringMessage> Sender { get; private set; }
        public PlantsAreas PlantsAreas { get; private set; }
        public IDictionary<string, IList<MeasuringMessage>> MessagesDictionary;
        public const int MessagesLimit = 2;

        public Observer(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;
           
            MessagesDictionary = new Dictionary<string, IList<MeasuringMessage>>();
            PlantsAreas.AllPlantsAreas.ToList().ForEach(pa => MessagesDictionary.Add(pa.Id, new List<MeasuringMessage>()));
        }
        
        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<MeasuringMessage> messengingEventArgs = eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
                MessagesDictionary[recievedMessage.PlantsAreaId].Add(recievedMessage);

                if (recievedMessage.MessageType == MessageTypesEnum.CriticalInfo)
                {
                    Console.WriteLine(recievedMessage.ToString());
                    Console.WriteLine("Message is accepted (Observer)!");
                }

                if (MessagesDictionary[recievedMessage.PlantsAreaId]
                    .Count(m => m.MessageType == MessageTypesEnum.CriticalInfo) >= MessagesLimit)
                {
                    SchedulingMessageCreator schedulingMessageCreator = 
                        new SchedulingMessageCreator(MessagesDictionary[recievedMessage.PlantsAreaId], recievedMessage.PlantsAreaId);

                    SchedulingMessage messageToSend = schedulingMessageCreator.CreateMessage();
                    //sending to scheduler
                    OnMessageSending(messageToSend);

                    MessagesDictionary[recievedMessage.PlantsAreaId].Clear();
                }
            }
        }

        public event EventHandler MessageSending;
        
        public void OnMessageSending(SchedulingMessage message)
        {
            EventHandler handler = MessageSending;
            if (handler != null)
            {
                handler(this, new MessengingEventArgs<SchedulingMessage>(message));
            }
        }
    }
}

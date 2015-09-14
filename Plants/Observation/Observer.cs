using System;
using System.Collections.Generic;
using System.Linq;
using Planting.MeasuringsProviding;
using Planting.MessagesCreators;
using Planting.Messenging;
using Planting.Plants;
using Planting.Timers;

namespace Planting.Observation
{
    public class Observer : IReciever, ISender<MeasuringMessage>
    {
        public ISender<MeasuringMessage> Sender { get; private set; }
        public PlantsAreas PlantsAreas { get; private set; }
        public IDictionary<string, IList<MeasuringMessage>> MessagesDictionary;
        public const int MessagesLimit = 10;

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
            MessengingEventArgs<MeasuringMessage> messengingEventArgs =
                eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
                MessagesDictionary[recievedMessage.PlantsAreaId].Add(recievedMessage);

                Console.WriteLine(recievedMessage.ToString());
                Console.WriteLine("{0} Elapsed", SystemTimer.CurrentTimeSpan);

                if (recievedMessage.MessageType == MessageTypesEnum.CriticalInfo)
                {
                    //sending to scheduler
                    OnMessageSending(recievedMessage);
                }

                if (MessagesDictionary[recievedMessage.PlantsAreaId].Count >= MessagesLimit)
                {
                    MessagesDictionary[recievedMessage.PlantsAreaId].Clear();
                    // to Db
                }
            }
        }

        public event EventHandler MessageSending;
        
        public void OnMessageSending(MeasuringMessage message)
        {
            EventHandler handler = MessageSending;
            if (handler != null)
            {
                handler(this, new MessengingEventArgs<MeasuringMessage>(message));
            }
        }
    }
}

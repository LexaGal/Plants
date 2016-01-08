using System;
using PlantingLib.MeasuringsProviders;
using PlantingLib.Messenging;
using PlantingLib.Observation;

namespace MongoDbServer
{
    public class MessagesListener : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }

        public MessagesListener(ISender<MeasuringMessage> sender)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;
        }

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<MeasuringMessage> messengingEventArgs =
                eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
            }
        }
    }
}
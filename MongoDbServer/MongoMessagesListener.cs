using System;
using MongoDbServer.MongoDocs;
using PlantingLib.MeasuringsProviders;
using PlantingLib.Messenging;
using PlantingLib.Observation;

namespace MongoDbServer
{
    public class MongoMessagesListener : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }

        public MongoMessagesListener(ISender<MeasuringMessage> sender)
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
                MongoMessage mongoMessage = new MongoMessage(recievedMessage);

                MongoDbAccessor mongoDbAccessor = new MongoDbAccessor();
                mongoDbAccessor.ConnectToMongoDatabase();

                mongoDbAccessor.AddMongoMessage(mongoMessage);
            }
        }
    }
}
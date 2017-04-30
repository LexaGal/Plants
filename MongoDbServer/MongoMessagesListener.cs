using System;
using MongoDbServer.MongoDocs;
using ObservationUtil;
using PlantingLib.Messenging;

namespace MongoDbServer
{
    public class MongoMessagesListener : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }
        public MongoDbAccessor MongoDbAccessor { get; private set; }
        public MongoMessagesListener(ISender<MeasuringMessage> sender)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            MongoDbAccessor = new MongoDbAccessor();
        }

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<MeasuringMessage> messengingEventArgs =
                eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
                if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                {
                    MongoMessage mongoMessage = new MongoMessage(recievedMessage);
                    MongoDbAccessor.AddMongoMessage(mongoMessage);
                }
            }
        }
    }
}
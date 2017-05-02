using System;
using MongoDbServer.MongoDocs;
using ObservationUtil;
using PlantingLib.Messenging;

namespace MongoDbServer
{
    public class MongoMessagesListener : IReciever
    {
        public MongoMessagesListener(ISender<MeasuringMessage> sender)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            MongoDbAccessor = new MongoDbAccessor();
        }

        public ISender<MeasuringMessage> Sender { get; private set; }
        public MongoDbAccessor MongoDbAccessor { get; }

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            var messengingEventArgs =
                eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                var recievedMessage = messengingEventArgs.Object;
                if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                {
                    var mongoMessage = new MongoMessage(recievedMessage);
                    MongoDbAccessor.AddMongoMessage(mongoMessage);
                }
            }
        }
    }
}
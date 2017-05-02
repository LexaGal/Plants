using System;
using System.Collections.Generic;
using System.Linq;
using AspNet.Identity.MySQL.Repository.Concrete;
using Database.MappingTypes;
using NLog;
using ObservationUtil;
using PlantingLib.Messenging;
using PlantingLib.Plants;

namespace PlantingLib.Observation
{
    public class Observer : IReciever, ISender<MeasuringMessage>
    {
        private const int MessagesLimit = 10;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        //private readonly IMeasuringMessageMappingRepository _measuringMessageMappingRepository;
        private readonly MySqlMeasuringMessageMappingRepository _sqlMeasuringMessageMappingRepository;
        public Dictionary<Guid, List<MeasuringMessage>> MessagesDictionary;

        public Observer(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;

            MessagesDictionary = new Dictionary<Guid, List<MeasuringMessage>>();

            PlantsAreas.Areas.ToList().ForEach(pa => MessagesDictionary.Add(pa.Id, new List<MeasuringMessage>()));

            //_measuringMessageMappingRepository = new MeasuringMessageMappingRepository();
            _sqlMeasuringMessageMappingRepository = new MySqlMeasuringMessageMappingRepository();
        }

        public PlantsAreas PlantsAreas { get; }


        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            try
            {
                var messengingEventArgs =
                    eventArgs as MessengingEventArgs<MeasuringMessage>;
                if (messengingEventArgs != null)
                {
                    var recievedMessage = messengingEventArgs.Object;

                    if (recievedMessage == null)
                        throw new ArgumentNullException(nameof(sender));

                    AddMeasuringMessage(recievedMessage);

                    if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                    {
                        //sending to scheduler
                        OnMessageSending(recievedMessage);

                        var area = PlantsAreas.Areas.SingleOrDefault(p => p.Id == recievedMessage.PlantsAreaId);

                        var sensor =
                            area?.Sensors.SingleOrDefault(s => s.MeasurableType == recievedMessage.MeasurableType);
                        if (sensor != null)
                            sensor.NumberOfTimes++;
                    }

                    if (MessagesDictionary[recievedMessage.PlantsAreaId].Count%MessagesLimit == 0)
                    {
                        List<MeasuringMessage> measuringMessages;
                        lock (MessagesDictionary)
                        {
                            measuringMessages = MessagesDictionary[recievedMessage.PlantsAreaId].Skip(
                                MessagesDictionary[recievedMessage.PlantsAreaId].Count - MessagesLimit).ToList();
                        }
                        SaveMessages(measuringMessages);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
        }

        public event EventHandler MessageSending;

        public void OnMessageSending(MeasuringMessage message)
        {
            var handler = MessageSending;
            handler?.Invoke(this, new MessengingEventArgs<MeasuringMessage>(message));
        }

        private void AddMeasuringMessage(MeasuringMessage measuringMessage)
        {
            if (!MessagesDictionary.ContainsKey(measuringMessage.PlantsAreaId))
                MessagesDictionary.Add(measuringMessage.PlantsAreaId, new List<MeasuringMessage>());
            MessagesDictionary[measuringMessage.PlantsAreaId].Add(measuringMessage);
        }

        private void SaveMessages(List<MeasuringMessage> measuringMessages)
        {
            if (measuringMessages != null)
            {
                var measuringMessageMappings = measuringMessages
                    .ConvertAll(message => new MeasuringMessageMapping(message.Id, message.DateTime,
                        message.MessageType.ToString(), message.MeasurableType,
                        message.PlantsAreaId, message.ParameterValue));

                _sqlMeasuringMessageMappingRepository.SaveMany(measuringMessageMappings);
            }
        }
    }
}
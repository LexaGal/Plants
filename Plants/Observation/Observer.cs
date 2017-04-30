using System;
using System.Collections.Generic;
using System.Linq;
using AspNet.Identity.MySQL.Repository.Concrete;
using Database.DatabaseStructure.Repository.Abstract;
using Database.MappingTypes;
using ObservationUtil;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Sensors;

namespace PlantingLib.Observation
{
    public class Observer : IReciever, ISender<MeasuringMessage>
    {
        public PlantsAreas PlantsAreas { get; }
        public Dictionary<Guid, List<MeasuringMessage>> MessagesDictionary;
        private const int MessagesLimit = 10;
        //private readonly IMeasuringMessageMappingRepository _measuringMessageMappingRepository;
        private readonly MySqlMeasuringMessageMappingRepository _sqlMeasuringMessageMappingRepository;

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

        private void AddMeasuringMessage(MeasuringMessage measuringMessage)
        {
            if (!MessagesDictionary.ContainsKey(measuringMessage.PlantsAreaId))
            {
                MessagesDictionary.Add(measuringMessage.PlantsAreaId, new List<MeasuringMessage>());
            }
            MessagesDictionary[measuringMessage.PlantsAreaId].Add(measuringMessage);
        }
    

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            try
            {
                MessengingEventArgs<MeasuringMessage> messengingEventArgs =
                    eventArgs as MessengingEventArgs<MeasuringMessage>;
                if (messengingEventArgs != null)
                {
                    MeasuringMessage recievedMessage = messengingEventArgs.Object;

                    if (recievedMessage == null)
                    {
                        throw new ArgumentNullException(nameof(sender));
                    }

                    AddMeasuringMessage(recievedMessage);

                    if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                    {
                        //sending to scheduler
                        OnMessageSending(recievedMessage);

                        PlantsArea area = PlantsAreas.Areas.SingleOrDefault(p => p.Id == recievedMessage.PlantsAreaId);

                        Sensor sensor =
                            area?.Sensors.SingleOrDefault(s => s.MeasurableType == recievedMessage.MeasurableType);
                        if (sensor != null)
                        {
                            sensor.NumberOfTimes++;
                        }
                    }

                    if (MessagesDictionary[recievedMessage.PlantsAreaId].Count % MessagesLimit == 0)
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
                //MessageBox.Show(e.StackTrace);
            }
        }

        private  void SaveMessages(List<MeasuringMessage> measuringMessages)
        {
            if (measuringMessages != null)
            {
                List<MeasuringMessageMapping> measuringMessageMappings = measuringMessages
                    .ConvertAll(message => new MeasuringMessageMapping(message.Id, message.DateTime,
                        message.MessageType.ToString(), message.MeasurableType,
                        message.PlantsAreaId, message.ParameterValue));

             _sqlMeasuringMessageMappingRepository.SaveMany(measuringMessageMappings);
            }
        }

        public event EventHandler MessageSending;
        
        public void OnMessageSending(MeasuringMessage message)
        {
            EventHandler handler = MessageSending;
            handler?.Invoke(this, new MessengingEventArgs<MeasuringMessage>(message));
        }
    }
}

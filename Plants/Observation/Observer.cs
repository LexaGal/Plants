using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Database.DatabaseStructure.Repository.Abstract;
using Database.DatabaseStructure.Repository.Concrete;
using Database.MappingTypes;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Sensors;

namespace PlantingLib.Observation
{
    public class Observer : IReciever, ISender<MeasuringMessage>
    {
        public PlantsAreas PlantsAreas { get; private set; }
        public IDictionary<Guid, IList<MeasuringMessage>> MessagesDictionary;
        private const int MessagesLimit = 30;
        private readonly IMeasuringMessageMappingRepository _measuringMessageMappingRepository;

        public Observer(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;

            MessagesDictionary = new Dictionary<Guid, IList<MeasuringMessage>>();
            PlantsAreas.Areas.ToList().ForEach(pa => MessagesDictionary.Add(pa.Id, new List<MeasuringMessage>()));

            _measuringMessageMappingRepository = new MeasuringMessageMappingRepository();
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
                        throw new ArgumentNullException("recievedMessage");
                    }

                    AddMeasuringMessage(recievedMessage);

                    if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                    {
                        //sending to scheduler
                        OnMessageSending(recievedMessage);

                        PlantsArea area = PlantsAreas.Areas.SingleOrDefault(p => p.Id == recievedMessage.PlantsAreaId);

                        if (area != null)
                        {
                            Sensor sensor =
                                area.Sensors.SingleOrDefault(s => s.MeasurableType == recievedMessage.MeasurableType);
                            if (sensor != null)
                            {
                                sensor.NumberOfTimes++;
                            }
                        }
                    }

                    if (MessagesDictionary[recievedMessage.PlantsAreaId].Count == MessagesLimit)
                    {
                        List<MeasuringMessage> measuringMessages =
                            MessagesDictionary[recievedMessage.PlantsAreaId].ToList();

                        SaveMessages(measuringMessages);

                        MessagesDictionary[recievedMessage.PlantsAreaId].Clear();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        private async void SaveMessages(List<MeasuringMessage> measuringMessages)
        {
            List<MeasuringMessageMapping> measuringMessageMappings = measuringMessages
                .ConvertAll(message => new MeasuringMessageMapping(message.Id, message.DateTime,
                    message.MessageType.ToString(), message.MeasurableType,
                    message.PlantsAreaId, message.ParameterValue));

            await _measuringMessageMappingRepository.SaveAsync(measuringMessageMappings);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Sensors;
using PlantingLib.Timers;

namespace PlantingLib.Observation
{
    public class Observer : IReciever, ISender<MeasuringMessage>
    {
        public PlantsAreas PlantsAreas { get; private set; }
        public IDictionary<Guid, IList<MeasuringMessage>> MessagesDictionary;
        public const int MessagesLimit = 10;

        public Observer(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;
           
            MessagesDictionary = new Dictionary<Guid, IList<MeasuringMessage>>();
            PlantsAreas.AllPlantsAreas.ToList().ForEach(pa => MessagesDictionary.Add(pa.Id, new List<MeasuringMessage>()));
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
                    if (!MessagesDictionary.ContainsKey(recievedMessage.PlantsAreaId))
                    {
                        MessagesDictionary.Add(recievedMessage.PlantsAreaId, new List<MeasuringMessage>());
                    }
                    MessagesDictionary[recievedMessage.PlantsAreaId].Add(recievedMessage);
                    
                    Console.WriteLine(recievedMessage.ToString());
                    Console.WriteLine("{0} Elapsed", SystemTimer.CurrentTimeSpan.TotalSeconds);

                    if (recievedMessage.MessageType == MessageTypeEnum.CriticalInfo)
                    {
                        //sending to scheduler
                        OnMessageSending(recievedMessage);
                        PlantsArea area = PlantsAreas.AllPlantsAreas.FirstOrDefault(p => p.Id == recievedMessage.PlantsAreaId);
                        if (area != null)
                        {
                            Sensor sensor = area.Sensors.FirstOrDefault(s => s.MeasurableType == recievedMessage.MeasurableType);
                            if (sensor != null)
                            {
                                sensor.NumberOfTimes++;
                            }
                        }
                    }

                    if (MessagesDictionary[recievedMessage.PlantsAreaId].Count >= MessagesLimit)
                    {
                        MessagesDictionary[recievedMessage.PlantsAreaId].Clear();
                        // to Db
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
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

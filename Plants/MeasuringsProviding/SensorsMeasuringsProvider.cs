using System;
using System.Linq;
using Planting.MessagesCreators;
using Planting.Messenging;
using Planting.Sensors;

namespace Planting.MeasuringsProviding
{
    public class SensorsMeasuringsProvider : ISender<MeasuringMessage>
    {
        public SensorsCollection Sensors { get; private set; }
        
        public SensorsMeasuringsProvider(SensorsCollection sensors)
        {
            Sensors = sensors;
        }

        public void SendMessages(TimeSpan timeSpan)
        {
            foreach (var sensor in Sensors.AllSensors.Where(sensor => sensor.IsOn))
            {
                if ((int) timeSpan.TotalSeconds%(int) sensor.MeasuringTimeout.TotalSeconds == 0)
                {
                    MeasuringMessageCreator measuringMessageCreator =
                        new MeasuringMessageCreator(sensor.MeasurableParameter,
                            sensor.PlantsArea.Id, sensor.GetCurrentMeasuring);

                    MeasuringMessage message = measuringMessageCreator.CreateMessage();
                    //sending to observer
                    OnMessageSending(message);
                }
            }
        }

        public event EventHandler MessageSending;
        
        public virtual void OnMessageSending(MeasuringMessage message)
        {
            EventHandler handler = MessageSending;
            if (handler != null)
            {
                handler(this, new MessengingEventArgs<MeasuringMessage>(message));
            }
        }
    }
}

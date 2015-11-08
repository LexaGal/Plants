using System;
using System.Linq;
using PlantingLib.MeasuringsProviding;
using PlantingLib.MessagesCreators;
using PlantingLib.Messenging;
using PlantingLib.Sensors;

namespace PlantingLib.MeasuringsProviders
{
    public class SensorsMeasuringsProvider : ISender<MeasuringMessage>
    {
        public SensorsCollection SensorCollection { get; private set; }
        
        public SensorsMeasuringsProvider(SensorsCollection sensorCollection)
        {
            SensorCollection = sensorCollection;
        }

        public void SendMessages(TimeSpan timeSpan)
        {
            foreach (Sensor sensor in SensorCollection.Sensors.Where(sensor => sensor.IsOn && !sensor.IsOffByUser))
            {
                if ((int) timeSpan.TotalSeconds%(int) sensor.MeasuringTimeout.TotalSeconds == 0)
                {
                    MeasuringMessageCreator measuringMessageCreator =
                        new MeasuringMessageCreator(sensor.MeasurableParameter,
                            sensor.PlantsArea.Id, sensor.GetNewMeasuring);

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

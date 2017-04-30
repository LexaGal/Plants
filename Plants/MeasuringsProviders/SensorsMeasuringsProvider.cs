using System;
using System.Linq;
using ObservationUtil;
using PlantingLib.MessagesCreators;
using PlantingLib.Messenging;
using PlantingLib.Sensors;
using PlantingLib.Timers;

namespace PlantingLib.MeasuringsProviders
{
    public class SensorsMeasuringsProvider : ISender<MeasuringMessage>
    {
        public SensorsCollection SensorCollection { get; }
        
        public SensorsMeasuringsProvider(SensorsCollection sensorCollection)
        {
            SensorCollection = sensorCollection;
        }

        public void SendMessages(TimeSpan timeSpan)
        {
            foreach (Sensor sensor in SensorCollection.Sensors.Where(sensor => sensor.IsOn && !sensor.IsOffByUser))
            {
                try
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
                catch (DivideByZeroException)
                {
                }
                catch (Exception)
                {                    
                }
            }
        }

        public event EventHandler MessageSending;
        
        public virtual void OnMessageSending(MeasuringMessage message)
        {
            EventHandler handler = MessageSending;
            handler?.Invoke(this, new MessengingEventArgs<MeasuringMessage>(message));
        }
    }
}

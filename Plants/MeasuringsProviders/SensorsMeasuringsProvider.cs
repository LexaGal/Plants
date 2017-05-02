using System;
using System.Linq;
using ObservationUtil;
using PlantingLib.MessagesCreators;
using PlantingLib.Messenging;
using PlantingLib.Timers;

namespace PlantingLib.MeasuringsProviders
{
    public class SensorsMeasuringsProvider : ISender<MeasuringMessage>
    {
        public SensorsMeasuringsProvider(SensorsCollection sensorCollection)
        {
            SensorCollection = sensorCollection;
        }

        public SensorsCollection SensorCollection { get; }

        public event EventHandler MessageSending;

        public virtual void OnMessageSending(MeasuringMessage message)
        {
            var handler = MessageSending;
            handler?.Invoke(this, new MessengingEventArgs<MeasuringMessage>(message));
        }

        public void SendMessages(TimeSpan timeSpan)
        {
            foreach (var sensor in SensorCollection.Sensors.Where(sensor => sensor.IsOn && !sensor.IsOffByUser))
                try
                {
                    if ((int) timeSpan.TotalSeconds%(int) sensor.MeasuringTimeout.TotalSeconds == 0)
                    {
                        var measuringMessageCreator =
                            new MeasuringMessageCreator(sensor.MeasurableParameter,
                                sensor.PlantsArea.Id, sensor.GetNewMeasuring);

                        var message = measuringMessageCreator.CreateMessage();

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
}
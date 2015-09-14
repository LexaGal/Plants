using System;
using System.Linq;
using Planting.MeasuringsProviding;
using Planting.Messenging;
using Planting.Observation;
using Planting.Plants;
using Planting.PlantsRequirements;

namespace Planting.ServiceSystems
{
    public class ServiceProvider : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }
        public PlantsAreas PlantsAreas { get; private set; }

        public ServiceProvider(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            Sender = sender;
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;
        }

        //recieving
        public void RecieveMessage(object sender, EventArgs eventArgs)
        {
            MessengingEventArgs<MeasuringMessage> messengingEventArgs =
                eventArgs as MessengingEventArgs<MeasuringMessage>;
            if (messengingEventArgs != null)
            {
                MeasuringMessage recievedMessage = messengingEventArgs.Object;
                Console.WriteLine("Message is accepted (ServiceProvider)!\n");

                ServiceSystem serviceSystem = GetServiceSystem(recievedMessage.MeasurableType, 
                    recievedMessage.ParameterValue, PlantsAreas.AllPlantsAreas.First(pa =>
                        pa.Id == recievedMessage.PlantsAreaId));

                TimeSpan timeSpan = serviceSystem.ComputeTimeForService();
                serviceSystem.StartService(timeSpan, GetServiceTime);
            }
        }

        public ServiceSystem GetServiceSystem(MeasurableTypesEnum measurableType,
            double parameterValue, PlantsArea plantsArea)
        {
            switch (measurableType)
            {
                    case MeasurableTypesEnum.Temperature:
                        return new TemperatureSystem(MeasurableTypesEnum.Temperature, parameterValue, plantsArea);
                    case MeasurableTypesEnum.Humidity:
                        return new TemperatureSystem(MeasurableTypesEnum.Humidity, parameterValue, plantsArea);
                    case MeasurableTypesEnum.Nutrient:
                        return new TemperatureSystem(MeasurableTypesEnum.Nutrient, parameterValue, plantsArea);
                    case MeasurableTypesEnum.SoilPh:
                        return new TemperatureSystem(MeasurableTypesEnum.SoilPh, parameterValue, plantsArea);
            }
            return null;
        }


        public TimeSpan GetServiceTime(TimeSpan timeSpan)
        {
            Console.WriteLine(timeSpan.TotalSeconds);
            return timeSpan;
        }
    }
}

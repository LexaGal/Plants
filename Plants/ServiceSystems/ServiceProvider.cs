using System;
using System.Linq;
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Messenging;
using PlantingLib.Observation;
using PlantingLib.Plants;

namespace PlantingLib.ServiceSystems
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
                
                ServiceSystem serviceSystem = GetServiceSystem(recievedMessage.MeasurableType, 
                    recievedMessage.ParameterValue, PlantsAreas.AllPlantsAreas.First(pa =>
                        pa.Id == recievedMessage.PlantsAreaId));

                serviceSystem.StartService(GetServiceTime);
            }
        }

        public ServiceSystem GetServiceSystem(MeasurableTypeEnum measurableType,
            double parameterValue, PlantsArea plantsArea)
        {
            switch (measurableType)
            {
                case MeasurableTypeEnum.Temperature:
                    return new TemperatureSystem(MeasurableTypeEnum.Temperature, parameterValue, plantsArea);
                case MeasurableTypeEnum.Humidity:
                    return new WaterSystem(MeasurableTypeEnum.Humidity, parameterValue, plantsArea);
                case MeasurableTypeEnum.Nutrient:
                    return new NutrientSystem(MeasurableTypeEnum.Nutrient, parameterValue, plantsArea);
                case MeasurableTypeEnum.SoilPh:
                    return new NutrientSystem(MeasurableTypeEnum.SoilPh, parameterValue, plantsArea);
            }
            return null;
        }


        public ServiceMessage GetServiceTime(ServiceMessage serviceMessage)
        {
            Console.WriteLine("\n{0}\n", serviceMessage);
            return serviceMessage;
        }
    }
}

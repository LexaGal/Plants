using System;
using System.Linq;
using System.Windows.Forms;
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
            try
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
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public ServiceSystem GetServiceSystem(string measurableType,
            double parameterValue, PlantsArea plantsArea)
        {
            switch (measurableType)
            {
                case "Temperature":
                    return new TemperatureSystem("Temperature", parameterValue, plantsArea);
                case "Humidity":
                    return new WaterSystem("Humidity", parameterValue, plantsArea);
                case "Nutrient":
                    return new NutrientSystem("Nutrient", parameterValue, plantsArea);
                case "SoilPh":
                    return new NutrientSystem("SoilPh", parameterValue, plantsArea);

            }
            return new CustomSystem(measurableType, parameterValue, plantsArea);
        }


        public ServiceMessage GetServiceTime(ServiceMessage serviceMessage)
        {
            Console.WriteLine("\n{0}\n", serviceMessage);
            return serviceMessage;
        }
    }
}

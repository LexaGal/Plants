using System;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using PlantingLib.MeasurableParameters;
using PlantingLib.MeasuringsProviding;
using PlantingLib.Messenging;
using PlantingLib.Observation;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantingLib.Timers;
using Timer = System.Timers.Timer;

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
            Timer timer = new Timer(1000);
            timer.Elapsed += ServiceTimer_Tick;
            timer.AutoReset = true;
            timer.Enabled = true;
        
        }

        private void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            foreach (PlantsArea area in PlantsAreas.AllPlantsAreas)
            {
                foreach (ServiceSchedule servicesSchedule in area.ServicesSchedulesState.ServicesSchedules)
                {
                    ServiceState serviceState =
                        area.PlantsAreaServiceState.ServiceStates.FirstOrDefault(
                            s => s.ServiceName == servicesSchedule.ServiceState.ToString());

                    // if service state is on -> ignore schedule, !!! set LastServicingTime = DateTime.Now;
                    if (serviceState != null && serviceState.IsOn == "✔")
                    {
                        servicesSchedule.LastServicingTime = DateTime.Now;
                        continue;
                    }

                    if (DateTime.Now.TimeOfDay.Subtract(servicesSchedule.LastServicingTime.TimeOfDay).TotalSeconds >=
                        servicesSchedule.ServicingPauseSpan.TotalSeconds)
                    {
                        servicesSchedule.LastServicingTime = DateTime.Now;
                        foreach (MeasurableParameter measurableParameter in servicesSchedule.MeasurableParameters)
                        {
                            Sensor sensor = area.Sensors.SingleOrDefault(
                                s => s.MeasurableParameter.MeasurableType == measurableParameter.MeasurableType);
                            if (sensor != null)
                            {
                                double parameterValue = sensor.Function.CurrentFunctionValue;

                                ServiceSystem serviceSystem = GetServiceSystem(measurableParameter.MeasurableType,
                                   parameterValue, area, servicesSchedule.ServicingSpan);

                                serviceSystem.StartService(GetServiceDescription);
                            }
                        }
                    }
                }
            }
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
                            pa.Id == recievedMessage.PlantsAreaId), TimeSpan.Zero);

                    serviceSystem.StartService(GetServiceDescription);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
        }

        public ServiceSystem GetServiceSystem(string measurableType, double parameterValue, PlantsArea plantsArea,
            TimeSpan servicingSpan)
        {
            ParameterEnum parameter;
            bool parsed = Enum.TryParse(measurableType, out parameter);

            if (parsed)
            {
                switch (parameter)
                {
                    case ParameterEnum.Temperature:
                        return new TemperatureSystem(ParameterEnum.Temperature.ToString(), parameterValue, plantsArea, servicingSpan);
                    case ParameterEnum.Humidity:
                        return new WaterSystem(ParameterEnum.Humidity.ToString(), parameterValue, plantsArea, servicingSpan);
                    case ParameterEnum.Nutrient:
                        return new NutrientSystem(ParameterEnum.Nutrient.ToString(), parameterValue, plantsArea, servicingSpan);
                    case ParameterEnum.SoilPh:
                        return new NutrientSystem(ParameterEnum.SoilPh.ToString(), parameterValue, plantsArea, servicingSpan);
                }
            }
            return new CustomSystem(measurableType, parameterValue, plantsArea, servicingSpan);
        }


        public ServiceMessage GetServiceDescription(ServiceMessage serviceMessage)
        {
            Console.WriteLine("\n{0}\n", serviceMessage);
            return serviceMessage;
        }
    }
}

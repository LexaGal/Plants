using System;
using System.Linq;
using System.Timers;
using ObservationUtil;
using PlantingLib.MeasurableParameters;
using PlantingLib.Messenging;
using PlantingLib.Plants;
using PlantingLib.Plants.ServicesScheduling;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using Timer = System.Timers.Timer;

namespace PlantingLib.ServiceSystems
{
    public class ServiceProvider : IReciever
    {
        public ISender<MeasuringMessage> Sender { get; private set; }
        public PlantsAreas PlantsAreas { get; }
        private readonly Timer _timer;

        public ServiceProvider(ISender<MeasuringMessage> sender, PlantsAreas plantsAreas)
        {
            Sender = sender;
            
            //subscribing
            sender.MessageSending += RecieveMessage;

            PlantsAreas = plantsAreas;
            _timer = new Timer(1000);
            _timer.Elapsed += ServiceTimer_Tick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        public void StopServices()
        {
            _timer.Stop();
        }

        public void StartServices()
        {
            _timer.Start();
        }

        private void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            foreach (PlantsArea area in PlantsAreas.Areas)
            {
                foreach (ServiceSchedule servicesSchedule in area.ServicesSchedulesStates
                    .ServicesSchedules.Where(schedule => schedule.IsOn))
                {
                    ServiceState serviceState =
                        area.PlantServicesStates.ServicesStates.FirstOrDefault(
                            s => s.ServiceName == servicesSchedule.ServiceName.ToString());

                    // if service state is on -> ignore schedule, !!! set LastServicingTime = DateTime.Now;
                    if (serviceState != null && serviceState.IsRunning)
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

                                ServiceMessage serviceMessage = serviceSystem.Service();
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
                        recievedMessage.ParameterValue, PlantsAreas.Areas.First(pa =>
                            pa.Id == recievedMessage.PlantsAreaId), TimeSpan.Zero);

                    ServiceMessage serviceMessage = serviceSystem.Service();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.StackTrace);
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
        
    }
}

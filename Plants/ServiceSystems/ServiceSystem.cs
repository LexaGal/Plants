using System;
using System.Linq;
using System.Timers;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Plants.ServiceStates;
using PlantingLib.Sensors;
using PlantingLib.Timers;

namespace PlantingLib.ServiceSystems
{
    public abstract class ServiceSystem
    {
        public PlantsArea PlantsArea { get; private set; }
        public string MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        
        private Timer _timer;
        protected TimeSpan ServiceTimeSpan;
        private MeasurableParameter _measurableParameter;
        private ServiceMessage _serviceMessage;
        
        protected ServiceSystem(string measurableType, double parameterValue, PlantsArea plantsArea, TimeSpan serviceTimeSpan)
        {
            PlantsArea = plantsArea;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
            ServiceTimeSpan = serviceTimeSpan;
        }

        public void SetServiceStateToOn(bool serviceIsOn)
        {
            Sensor sensor = PlantsArea.Sensors.SingleOrDefault(s => s.MeasurableParameter.MeasurableType == MeasurableType);
            if (sensor != null)
            {
                 sensor.IsOn = !serviceIsOn;
            }

            ParameterEnum parameter;
            bool parsed = Enum.TryParse(MeasurableType, out parameter);

            ServiceState serviceState = null;

            if (parsed)
            {
                switch (parameter)
                {
                    case ParameterEnum.Humidity:
                        serviceState = PlantsArea.PlantServicesStates.ServicesStates.First(
                            s => s.ServiceName == ServiceStateEnum.Watering.ToString());
                        break;
                    case ParameterEnum.Temperature:
                        if (ParameterValue < PlantsArea.Plant.Temperature.Optimal)
                        {
                            serviceState = PlantsArea.PlantServicesStates.ServicesStates.First(
                                s => s.ServiceName == ServiceStateEnum.Warming.ToString());
                            break;
                        }
                        serviceState = PlantsArea.PlantServicesStates.ServicesStates.First(
                            s => s.ServiceName == ServiceStateEnum.Cooling.ToString());
                        break;
                    case ParameterEnum.SoilPh:
                        serviceState = PlantsArea.PlantServicesStates.ServicesStates.First(
                            s => s.ServiceName == ServiceStateEnum.Nutrienting.ToString());
                        break;
                    case ParameterEnum.Nutrient:
                        serviceState = PlantsArea.PlantServicesStates.ServicesStates.First(
                            s => s.ServiceName == ServiceStateEnum.Nutrienting.ToString());
                        break;
                }
                
                if (serviceState != null)
                {
                    serviceState.IsOn = serviceIsOn.ToString();
                    serviceState.IsScheduled = (ServiceTimeSpan != TimeSpan.Zero && serviceIsOn).ToString();
                    return;
                }
            }

            //if custom service state
            serviceState = PlantsArea.PlantServicesStates.ServicesStates
                .FirstOrDefault(s => s.IsFor(MeasurableType));
            if (serviceState != null)
            {
                serviceState.IsOn = serviceIsOn.ToString();
                serviceState.IsScheduled = (ServiceTimeSpan != TimeSpan.Zero && serviceIsOn).ToString();
            }
        }

        public void ResetSensorFunction(double newFunctionValue)
        {
            Sensor sensor = PlantsArea.Sensors.SingleOrDefault(s => s.MeasurableParameter.MeasurableType == MeasurableType);
            if (sensor != null)
            {
                sensor.Function.ResetFunction(newFunctionValue);
            }
        }

        public abstract TimeSpan ComputeTimeForService();

        public void ServiceTime_Elapsed(object sender, ElapsedEventArgs e)
        {
            _measurableParameter = PlantsArea.Plant.GetMeasurableParameter(MeasurableType);
            if (_measurableParameter != null)
            {
                if (SystemTimer.IsEnabled)
                {
                    // TimeSpan.Zero is for service of message SOS, not of schedule
                    if (ServiceTimeSpan == TimeSpan.Zero)
                    {

                        ServiceMessage serviceMessage = new ServiceMessage(PlantsArea.Id, MeasurableType,
                            _measurableParameter.Optimal, new TimeSpan((int) _timer.Interval));

                        _serviceMessage = serviceMessage;
                    }

                    ResetSensorFunction(_measurableParameter.Optimal);
                }
                else
                {
                    if (ParameterValue > _measurableParameter.Optimal)
                    {
                        ResetSensorFunction(ParameterValue - ServiceTimeSpan.TotalSeconds);
                    }
                    else
                    {
                        ResetSensorFunction(ParameterValue + ServiceTimeSpan.TotalSeconds);
                    }
                }
                SetServiceStateToOn(false);
            }
        }

        public ServiceMessage Service()
        {
            SetServiceStateToOn(true);

            _timer = new Timer(ComputeTimeForService().TotalMilliseconds);
            _timer.Elapsed += ServiceTime_Elapsed;
            _timer.AutoReset = false;
            _timer.Enabled = true;

            return _serviceMessage;
        }
    }
}
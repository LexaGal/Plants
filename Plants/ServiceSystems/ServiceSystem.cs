using System;
using System.Linq;
using System.Timers;
using PlantingLib.MeasurableParameters;
using PlantingLib.Plants;
using PlantingLib.Timers;

namespace PlantingLib.ServiceSystems
{
    public abstract class ServiceSystem
    {
        public PlantsArea PlantsArea { get; private set; }
        public string MeasurableType { get; private set; }
        public double ParameterValue { get; private set; }
        private Timer _timer;
        private Func<ServiceMessage, ServiceMessage> _func;
        private int _nIntervals;
        
        protected ServiceSystem(string measurableType, double parameterValue, PlantsArea plantsArea)
        {
            PlantsArea = plantsArea;
            MeasurableType = measurableType;
            ParameterValue = parameterValue;
        }

        public void SetSensorsState(bool state)
        {
            PlantsArea.Sensors
                .Where(s => s.MeasurableParameter.MeasurableType == MeasurableType)
                .ToList()
                .ForEach(s => s.IsOn = state);
            switch (MeasurableType)
            {
                case "Humidity":
                    PlantsArea.PlantsAreaServiceState.ServiceStates.First(s => s.ServiceName == "Watering").IsOn =
                        (!state).ToString();
                    return;
                case "Temperature":
                    if (ParameterValue < PlantsArea.Plant.Temperature.Optimal)
                    {
                        PlantsArea.PlantsAreaServiceState.ServiceStates.First(s => s.ServiceName == "Warming").IsOn =
                            (!state).ToString();
                        return;
                    }
                    PlantsArea.PlantsAreaServiceState.ServiceStates.First(s => s.ServiceName == "Cooling").IsOn =
                        (!state).ToString();
                    return;
                case "SoilPh":
                    PlantsArea.PlantsAreaServiceState.ServiceStates.First(s => s.ServiceName == "Nutrienting").IsOn =
                        (!state).ToString();
                    return;
                case "Nutrient":
                    PlantsArea.PlantsAreaServiceState.ServiceStates.First(s => s.ServiceName == "Nutrienting").IsOn =
                        (!state).ToString();
                    return;
            }
            //if custom service state
            PlantsArea.PlantsAreaServiceState.ServiceStates
                .First(s => s.ServiceName.Contains(MeasurableType))
                .IsOn = (!state).ToString();
        }

        public void ResetSensorsFunctions()
        {
            PlantsArea.Sensors
                .Where(s => s.MeasurableParameter.MeasurableType == MeasurableType)
                .ToList()
                .ForEach(s => s.Function.ResetFunction());
        }

        public abstract TimeSpan ComputeTimeForService();

        public void ServiceTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (SystemTimer.IsEnabled)
            {
                if (_nIntervals >= (int) ComputeTimeForService().TotalSeconds)
                {
                    ServiceMessage serviceMessage = new ServiceMessage(PlantsArea.Id, MeasurableType,
                        PlantsArea.Plant.GetMeasurableParameter(MeasurableType).Optimal, ComputeTimeForService());

                    _func(serviceMessage);

                    ResetSensorsFunctions();

                    SetSensorsState(true);
                    
                    _timer.Enabled = false;
                    _timer.AutoReset = false;

                    _nIntervals = 0;
                    return;
                }

                _nIntervals++;
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
            else
            {
                _timer.AutoReset = true;
                _timer.Enabled = true;
            }
        }

        public void StartService(Func<ServiceMessage, ServiceMessage> func)
        {
            SetSensorsState(false);

            _nIntervals = 0;
            _func = func;
            
            _timer = new Timer(1000);
            _timer.Elapsed += ServiceTimer_Tick;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }
    }
}